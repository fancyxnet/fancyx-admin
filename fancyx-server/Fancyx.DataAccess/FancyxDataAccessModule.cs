using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fancyx.DataAccess
{
    public class FancyxDataAccessModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDbContext<FancyxDbContext>((sp, options) =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("Default"))
                     .LogTo(Console.WriteLine, LogLevel.Information)
#if DEBUG
                     .EnableSensitiveDataLogging()
#endif
                     .EnableDetailedErrors(); ;
            });
            //context.Services.AddScoped<UnitOfWorkManager>(r => new UnitOfWorkManager(createFreeSql()));
            context.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
    }
}