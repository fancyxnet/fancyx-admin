using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fancyx.Admin.EfCore
{
    public class FancyxAdminEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDbContext<DbContext, FancyxDbContext>((sp, options) =>
            {
                options.UseNpgsql(context.Configuration.GetConnectionString("Default"))
                     .LogTo(Console.WriteLine, LogLevel.Information)
#if DEBUG
                     .EnableSensitiveDataLogging()
#endif
                     .EnableDetailedErrors();
            }, ServiceLifetime.Scoped);
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}