using DotNetCore.CAP;
using Fancyx.Assemblies;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Connection;
using Fancyx.Core.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
                var tableSchema = context.Configuration["Cap:TableSchema"]!;
                var dbOptions = context.Configuration.GetRequiredSection("ConnectionStrings").Get<ConnectionStringOption>()!;
                var connectionString = dbOptions.GetConnectionString();
                switch(dbOptions.DatabaseType)
                {
                    case DbType.PostgreSql:
                        x.UsePostgreSql(opt =>
                        {
                            opt.Schema = tableSchema;
                            opt.DataSource = NpgsqlDataSource.Create(connectionString);
                        });
                        break;
                    case DbType.MySql:
                        x.UseMySql(opt =>
                        {
                            opt.TableNamePrefix = tableSchema;
                            opt.ConnectionString = connectionString;
                        });
                        break;
                }
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