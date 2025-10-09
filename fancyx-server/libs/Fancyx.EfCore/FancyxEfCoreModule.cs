using Fancyx.Core.AutoInject;
using Fancyx.Core.Connection;
using Fancyx.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Fancyx.EfCore
{
    public class FancyxEfCoreModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }

    public static class DbContextSetup
    {
        public static void AddEfCore<TDbContext>(this IServiceCollection services, IConfiguration configuration) where TDbContext : EfCoreDbContextBase
        {
            var dbOptions = configuration.GetRequiredSection("ConnectionStrings").Get<ConnectionStringOption>()!;
            var connectionString = dbOptions.GetConnectionString();
            services.AddDbContext<DbContext, TDbContext>(options
                =>
            {
                switch (dbOptions.DatabaseType)
                {
                    case DbType.PostgreSql:
                        options.UseNpgsql(connectionString)
                               .UseSnakeCaseNamingConvention();
                        break;
                    case DbType.MySql:
                        var version = ServerVersion.AutoDetect(connectionString);
                        options.UseMySql(connectionString, version)
                               .UseSnakeCaseNamingConvention();
                        break;
                }
                options.LogTo(Console.WriteLine, LogLevel.Information)
#if DEBUG
                       .EnableSensitiveDataLogging()
#endif
                       .EnableDetailedErrors();
            });
            services.TryAddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}