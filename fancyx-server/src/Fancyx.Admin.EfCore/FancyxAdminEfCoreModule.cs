using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;
using Fancyx.Shared.EfCore;

namespace Fancyx.Admin.EfCore
{
    [DependsOn(
        typeof(FancyxSharedEfCoreModule)
        )]
    public class FancyxAdminEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<FancyxDbContext>(context.Configuration);
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}