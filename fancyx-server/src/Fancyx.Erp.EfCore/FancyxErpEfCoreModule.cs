using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;
using Fancyx.Shared.EfCore;

namespace Fancyx.Erp.EfCore
{
    [DependsOn(
    typeof(FancyxSharedEfCoreModule)
    )]
    public class FancyxErpEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<FancyxErpDbContext>(context.Configuration);
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}
