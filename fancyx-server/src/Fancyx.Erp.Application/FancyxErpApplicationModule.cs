using Fancyx.Cache;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Erp.EfCore;
using Fancyx.EventBus;
using Fancyx.Shared.Logger;

namespace Fancyx.Erp.Application
{
    [DependsOn(
         typeof(FancyxCacheModule),
         typeof(FancyxEventBusModule),
         typeof(FancyxSharedLoggerModule),
         typeof(FancyxErpEfCoreModule)
        )]
    public class FancyxErpApplicationModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
