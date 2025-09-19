using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;

namespace Fancyx.Admin.EfCore
{
    [DependsOn(
        typeof(FancyxEfCoreModule)
        )]
    public class FancyxAdminEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}