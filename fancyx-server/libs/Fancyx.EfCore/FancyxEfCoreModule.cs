using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Microsoft.EntityFrameworkCore;
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
        public static void AddEfCore<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : EfCoreDbContextBase
        {
            services.AddDbContext<DbContext, TDbContext>(options
                => options.UseNpgsql(connectionString)
                     .LogTo(Console.WriteLine, LogLevel.Information)
#if DEBUG
                     .EnableSensitiveDataLogging()
#endif
                     .EnableDetailedErrors());
            services.TryAddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}