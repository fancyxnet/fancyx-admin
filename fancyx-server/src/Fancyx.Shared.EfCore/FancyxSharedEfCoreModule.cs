using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;

namespace Fancyx.Shared.EfCore
{
    [DependsOn(
        typeof(FancyxEfCoreModule)
        )]
    public class FancyxSharedEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}
