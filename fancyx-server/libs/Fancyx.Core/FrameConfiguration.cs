using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Fancyx.Core.Authorization;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Core.Interfaces;
using Fancyx.Core.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace Fancyx.Core
{
    public static class FrameConfiguration
    {
        /// <summary>
        /// 用于存储所有模块的集合（带顺序）
        /// </summary>
        private static ConcurrentDictionary<Type, (ModuleBase instance, int sort)> modules = [];

        /// <summary>
        /// 已加载程序集
        /// </summary>
        private static readonly List<Assembly> _loadAssemblies = [];

        /// <summary>
        /// 用于模块排序的计数器
        /// </summary>
        private static int sort = 0;

        /// <summary>
        /// 是否调用过标识，-1初始状态，等于1表示已经调用过<see cref="AddApplication"/>，等于2表示已经调用过<see cref="InitializeApplication"/>，
        /// </summary>
        private static int execution = -1;

        /// <summary>
        /// 添加应用程序配置，在Program.cs中调用1次
        /// </summary>
        /// <typeparam name="T">Host模块类</typeparam>
        /// <param name="builder"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplication<T>(this WebApplicationBuilder builder) where T : ModuleBase
        {
            if (Interlocked.CompareExchange(ref execution, 1, -1) == 1)
            {
                throw new InvalidOperationException("AddApplication方法在单个服务中只能调用1次");
            }

            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddControllers().AddNewtonsoftJson();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddConnections();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }); //关闭默认参数验证

            services.AddJwt(configuration);
            services.AddScoped<ICurrentUser>(sp =>
            {
                return CurrentUser.Parse(sp.GetRequiredService<IHttpContextAccessor>()?.HttpContext);
            }); //当前用户
            services.AddScoped<ICurrentTenant>(sp =>
            {
                return CurrentTenant.Parse(sp.GetRequiredService<IHttpContextAccessor>()?.HttpContext);
            }); //当前租户

            //1. 扫描模块，调用ConfigureServices方法
            ServiceConfigurationContext context = new ServiceConfigurationContext(builder.Services, builder.Configuration);
            Type mainType = typeof(T);
            ModuleBase? mainModule = (ModuleBase?)Activator.CreateInstance(mainType);
            if (mainModule == null) return;
            InjectModule(context, mainModule);

            //2. 获取加载程序集
            foreach (var item in modules.Keys)
            {
                _loadAssemblies.Add(item.Assembly);
            }
            //3. Autofac动态注册
            builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureAutofacContainer);
            //4. 注册AutoMapper
            services.AddAutoMapper(_loadAssemblies);
        }

        /// <summary>
        /// 初始化应用程序配置，在Program.cs中调用1次
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void InitializeApplication(this WebApplication app)
        {
            if (execution == -1)
            {
                throw new InvalidOperationException("请先调用AddApplication方法");
            }
            if (Interlocked.CompareExchange(ref execution, 2, 1) == 2)
            {
                throw new InvalidOperationException("InitializeApplication方法在单个服务中只能调用1次");
            }

            app.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{param:regex(.*+)}");
            app.UseRouting();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMiddleware<MultiTenancyMiddleware>();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CurrentUserMiddleware>();

            var context = new ApplicationInitializationContext(app);
            foreach (var module in modules.OrderBy(m => m.Value.sort))
            {
                module.Value.instance.Configure(context);
            }

            modules = null!;
        }

        /// <summary>
        /// 使用Autofac作为DI容器
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ConfigureHostBuilder UseAutofac(this ConfigureHostBuilder host)
        {
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureContainer<ContainerBuilder>(builder =>
               {
                   //拦截器
                   builder.RegisterType<AopAttributeInterceptor>().AsSelf();
                   builder.RegisterType<AsyncAopAttributeInterceptor>().As<IAsyncInterceptor>();
                   builder.RegisterType<AsyncInterceptorAdaper>().AsSelf();
               });
            return host;
        }

        private static void InjectModule(ServiceConfigurationContext context, ModuleBase module)
        {
            Type curModuleType = module.GetType();
            DependsOnAttribute? dependsOnAttribute = curModuleType.GetCustomAttribute<DependsOnAttribute>();
            if (dependsOnAttribute != null)
            {
                foreach (var moduleType in dependsOnAttribute.DependedModuleTypes)
                {
                    if (moduleType.Equals(curModuleType)) continue; //避免循环依赖
                    ModuleBase? subModule = (ModuleBase?)Activator.CreateInstance(moduleType);
                    if (subModule == null) continue;

                    InjectModule(context, subModule);
                }
            }

            if (modules.ContainsKey(curModuleType)) return;
            if (module.Order >= 0)
            {
                modules.TryAdd(curModuleType, (module, module.Order));
            }
            else
            {
                Interlocked.Increment(ref sort);
                modules.TryAdd(curModuleType, (module, sort));
            }
            module.ConfigureServices(context);
        }

        /// <summary>
        /// 增加JWT认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(configuration.GetSection("Jwt")["ClockSkew"])),
                        ValidateIssuerSigningKey = true,
                        ValidAudience = configuration.GetSection("Jwt")["ValidAudience"],
                        ValidIssuer = configuration.GetSection("Jwt")["ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt")["IssuerSigningKey"]!))
                    };
                });
        }

        /// <summary>
        /// 按照依赖模块类型注册到Autofac容器中
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureAutofacContainer(ContainerBuilder builder)
        {
            var singletonServiceType = typeof(ISingletonDependency);
            var scopedServiceType = typeof(IScopedDependency);
            var transientServiceType = typeof(ITransientDependency);
            var denpendencyInjectType = typeof(DenpendencyInjectAttribute);
            var baseTypeMap = new Dictionary<DenpendencyType, Type>
            {
                [DenpendencyType.Singleton] = singletonServiceType,
                [DenpendencyType.Scoped] = scopedServiceType,
                [DenpendencyType.Transient] = transientServiceType
            };
            var autowireType = typeof(AutowiredAttribute);
            foreach (var assembly in _loadAssemblies)
            {
                //实现注册接口类注册
                var curClassTypes = assembly.DefinedTypes.Where(x => !x.IsAbstract && x.IsClass && !x.IsSealed).ToList();
                foreach (var baseType in baseTypeMap)
                {
                    var curBaseTypes = curClassTypes.Where(x => x != baseType.Value && x.IsAssignableTo(baseType.Value) && !x.IsDefined(denpendencyInjectType)).ToList();
                    curBaseTypes.ForEach(c =>
                    {
                        RegisterType(c, baseType.Key);
                    });
                }
                var curAttrTypes = curClassTypes.Where(x => x.IsDefined(denpendencyInjectType)).ToList();
                //标记注册特性类注册
                foreach (var attrType in curAttrTypes)
                {
                    var attr = attrType.GetCustomAttribute<DenpendencyInjectAttribute>();
                    if (attr == null) continue;
                    if (!attr.AsSelf && (attr.Interfaces == null || attr.Interfaces.Length <= 0)) continue;
                    RegisterType(attrType, attr.Way, attr.AsSelf, attr.Interfaces);
                }
            }

            void RegisterType(TypeInfo classType, DenpendencyType denpendencyType, bool asSelf = false, Type[]? interfaces = default)
            {
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder;
                var implementedInterfaces = interfaces != null && interfaces.Length > 0 ? interfaces : classType.ImplementedInterfaces.Where(x => x != singletonServiceType && x != scopedServiceType && x != transientServiceType).ToArray();
                if (implementedInterfaces.Length > 0 && !asSelf)
                {
                    registrationBuilder = builder.RegisterType(classType).As(implementedInterfaces).EnableInterfaceInterceptors().InterceptedBy(typeof(AopAttributeInterceptor), typeof(AsyncInterceptorAdaper));
                }
                else
                {
                    registrationBuilder = builder.RegisterType(classType).AsSelf();
                }

                //标记AutowiredAttribute的属性注入
                registrationBuilder.PropertiesAutowired((propInfo, instance) => propInfo.IsDefined(autowireType));

                switch (denpendencyType)
                {
                    case DenpendencyType.Singleton:
                        registrationBuilder.SingleInstance();
                        break;

                    case DenpendencyType.Transient:
                        registrationBuilder.InstancePerLifetimeScope();
                        break;

                    default:
                        registrationBuilder.InstancePerDependency();
                        break;
                }
            }
        }
    }
}