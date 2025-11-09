using DotNetCore.CAP;

using Fancyx.Assemblies;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

using System.Reflection;

namespace Fancyx.EventBus
{
    public class FancyxEventBusModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddCap(x =>
            {
                x.UseRedis(opt =>
                {
                    opt.Configuration = ConfigurationOptions.Parse(context.Configuration["Cap:RedisConnection"]!);
                });
                x.UseMySql(opt =>
                {
                    opt.TableNamePrefix = context.Configuration["Cap:TableSchema"]!;
                    opt.ConnectionString = context.Configuration["Cap:DbConnection"]!;
                });
#if DEBUG
                x.UseDashboard(options =>
                {
                    options.PathBase = context.Configuration["Cap:PathBase"] ?? "/Cap";
                });
#endif
            });

            //自动以Scoped方式注册ICapSubscribe实现类
            var baseType = typeof(ICapSubscribe);
            foreach (var assembly in AssemblyLoader.All)
            {
                IEnumerable<TypeInfo> types = assembly.DefinedTypes.Where(x => !x.IsAbstract && x.IsClass && !x.IsSealed && x != baseType && x.IsAssignableTo(baseType));
                foreach (var type in types)
                {
                    context.Services.AddScoped(type);
                }
            }
        }

        public override void Configure(ApplicationInitializationContext app)
        {
        }
    }
}