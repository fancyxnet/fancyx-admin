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
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Fancyx.Core
{
    public static class FrameConfiguration
    {
        /// <summary>
        /// 用于存储所有模块的集合（带顺序）
        /// </summary>
        private static ConcurrentDictionary<Type, (ModuleBase instance, int sort)> _modules = [];

        /// <summary>
        /// 已加载程序集
        /// </summary>
        private static readonly List<Assembly> LoadAssemblies = [];

        /// <summary>
        /// 用于模块排序的计数器
        /// </summary>
        private static int _sort;

        /// <summary>
        /// 是否调用过标识，-1初始状态，等于1表示已经调用过<see cref="AddApplication"/>，等于2表示已经调用过<see cref="InitializeApplication"/>，
        /// </summary>
        private static int _execution = -1;

        /// <summary>
        /// 添加应用程序配置，在Program.cs中调用1次
        /// </summary>
        /// <typeparam name="T">Host模块类</typeparam>
        /// <param name="builder"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplication<T>(this WebApplicationBuilder builder) where T : ModuleBase
        {
            if (Interlocked.CompareExchange(ref _execution, 1, -1) == 1)
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

            services.AddScoped<ICurrentUser>(sp => CurrentUser.Parse(sp.GetRequiredService<IHttpContextAccessor>().HttpContext)); //当前用户
            services.AddScoped<ICurrentTenant>(sp => CurrentTenant.Parse(sp.GetRequiredService<IHttpContextAccessor>().HttpContext)); //当前租户

            //1. 扫描模块，调用ConfigureServices方法
            var context = new ServiceConfigurationContext(builder.Services, builder.Configuration);
            var mainType = typeof(T);
            var mainModule = (ModuleBase?)Activator.CreateInstance(mainType);
            if (mainModule == null) return;

            var stopWatch = Stopwatch.StartNew();
            InjectModule(context, mainModule);
            Console.WriteLine("加载依赖模块耗时{0}ms", stopWatch.ElapsedMilliseconds);
            stopWatch.Stop();

            //2. 获取加载程序集
            foreach (var item in _modules.Keys)
            {
                LoadAssemblies.Add(item.Assembly);
            }
            //3. 原生动态注册
            ConfigureNativeDIContainer(services);
            //4. 注册AutoMapper
            services.AddAutoMapper(LoadAssemblies);
        }

        /// <summary>
        /// 初始化应用程序配置，在Program.cs中调用1次
        /// </summary>
        /// <param name="app"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void InitializeApplication(this WebApplication app)
        {
            if (_execution == -1)
            {
                throw new InvalidOperationException("请先调用AddApplication方法");
            }
            if (Interlocked.CompareExchange(ref _execution, 2, 1) == 2)
            {
                throw new InvalidOperationException("InitializeApplication方法在单个服务中只能调用1次");
            }

            app.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{param:regex(.*+)}");
            app.UseRouting();

            app.Use(async (ctx, next) =>
            {
                ServiceScopeAccessor.Set(ctx.RequestServices);
                await next();
            });

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMiddleware<MultiTenancyMiddleware>();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CurrentUserMiddleware>();

            var context = new ApplicationInitializationContext(app);
            foreach (var module in _modules.OrderBy(m => m.Value.sort))
            {
                module.Value.instance.Configure(context);
            }

            _modules = null!;
        }

        private static void InjectModule(ServiceConfigurationContext context, ModuleBase module)
        {
            var curModuleType = module.GetType();

            if (_modules.ContainsKey(curModuleType)) return;

            var dependsOnAttribute = curModuleType.GetCustomAttribute<DependsOnAttribute>();
            if (dependsOnAttribute != null)
            {
                foreach (var moduleType in dependsOnAttribute.DependedModuleTypes)
                {
                    if (moduleType == curModuleType) continue; //避免循环依赖

                    var subModule = _modules.TryGetValue(moduleType, out var subModuleValue) ? subModuleValue.instance : (ModuleBase?)Activator.CreateInstance(moduleType);
                    if (subModule == null) continue;

                    InjectModule(context, subModule);
                }
            }

            if (module.Order >= 0)
            {
                _modules.TryAdd(curModuleType, (module, module.Order));
            }
            else
            {
                Interlocked.Increment(ref _sort);
                _modules.TryAdd(curModuleType, (module, _sort));
            }

            if (_modules[curModuleType].instance.Equals(module))
            {
                module.ConfigureServices(context);
            }
        }

        /// <summary>
        /// 增加JWT认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
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
        /// 按照依赖模块类型注册到容器中
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureNativeDIContainer(IServiceCollection services)
        {
            var stopWatch = Stopwatch.StartNew();

            services.AddSingleton<IAsyncInterceptor, AopAttributeInterceptor>();

            var singletonServiceType = typeof(ISingletonDependency);
            var scopedServiceType = typeof(IScopedDependency);
            var transientServiceType = typeof(ITransientDependency);
            var denpendencyInjectType = typeof(DependencyInjectAttribute);

            // 使用 Scrutor 批量注册实现生命周期接口的类（排除标记接口）
            var markerInterfaces = new[] { singletonServiceType, scopedServiceType, transientServiceType };

            services.Scan(scan => scan
                .FromAssemblies(LoadAssemblies)
                .AddClasses(classes => classes
                    .Where(t => !t.IsAbstract && !t.IsSealed && !t.IsInterface && t.GetInterfaces().Any(i => i == singletonServiceType || i == scopedServiceType || i == transientServiceType))
                    .Where(t => !t.IsDefined(denpendencyInjectType))
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .As(type => type.GetInterfaces().Where(i => !markerInterfaces.Contains(i)))
                .WithLifetime(type =>
                {
                    if (singletonServiceType.IsAssignableFrom(type)) return ServiceLifetime.Singleton;
                    if (scopedServiceType.IsAssignableFrom(type)) return ServiceLifetime.Scoped;
                    return ServiceLifetime.Transient;
                })
            );

            // 处理带DenpendencyInjectAttribute的类型，增加装饰器
            foreach (var assembly in LoadAssemblies)
            {
                var classes = assembly.DefinedTypes.Where(t => !t.IsAbstract && !t.IsSealed && !t.IsInterface);
                var attrTypes = classes.Where(t => t.IsDefined(denpendencyInjectType));

                foreach (var type in attrTypes)
                {
                    var attr = type.GetCustomAttribute<DependencyInjectAttribute>();
                    if (attr == null) continue;
                    if (!attr.AsSelf && (attr.Interfaces == null || attr.Interfaces.Length <= 0)) continue;

                    if (attr.AsSelf)
                    {
                        services.Add(new ServiceDescriptor(type, type, attr.Way));
                    }
                    else
                    {
                        foreach (var iface in attr.Interfaces!)
                        {
                            services.Add(new ServiceDescriptor(iface, type, attr.Way));
                        }
                    }
                }
            }

            var serviceTypes = services.Where(sd => sd.ImplementationType != null && !sd.ServiceType.IsGenericTypeDefinition && sd.ServiceType.IsInterface).Select(sd => sd.ServiceType).Distinct().ToList();
            foreach (var serviceType in serviceTypes)
            {
                if (!LoadAssemblies.Any(a => serviceType.Assembly == a)) continue;

                if (services.Any(sd => sd.ServiceType == serviceType))
                {
                    services.Decorate(serviceType, (inner, provider) =>
                    {
                        var interceptor = provider.GetRequiredService<IAsyncInterceptor>();
                        var proxyGenerator = new ProxyGenerator();
                        return proxyGenerator.CreateInterfaceProxyWithTarget(
                            serviceType,
                            inner,
                            new AsyncDeterminationInterceptor(interceptor)
                        );
                    });
                }
            }

            Console.WriteLine("服务注册扫描耗费{0}ms", stopWatch.ElapsedMilliseconds);
            stopWatch.Stop();
        }
    }
}