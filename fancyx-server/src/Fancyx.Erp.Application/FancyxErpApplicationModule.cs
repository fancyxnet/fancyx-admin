using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Fancyx.Erp.EfCore;
using Fancyx.Shared.Logger;

namespace Fancyx.Erp.Application
{
    [DependsOn(
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
