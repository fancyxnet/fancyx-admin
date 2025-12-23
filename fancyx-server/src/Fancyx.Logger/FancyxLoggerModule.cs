using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;
using Fancyx.Shared.Logger;

namespace Fancyx.Logger
{
    [DependsOn(
        typeof(FancyxEfCoreModule),
        typeof(FancyxSharedLoggerModule)
    )]
    public class FancyxLoggerModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<LoggerDbContext>(context.Configuration);
        }
    }
}
