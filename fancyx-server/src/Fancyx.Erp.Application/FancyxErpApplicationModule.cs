using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Erp.EfCore;
using Fancyx.EventBus;
using Fancyx.Redis;
using Fancyx.Shared.Logger;

namespace Fancyx.Erp.Application
{
    [DependsOn(
         typeof(FancyxRedisModule),
         typeof(FancyxEventBusModule),
         typeof(FancyxSharedLoggerModule),
         typeof(FancyxErpEfCoreModule)
        )]
    public class FancyxErpApplicationModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
            throw new NotImplementedException();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
