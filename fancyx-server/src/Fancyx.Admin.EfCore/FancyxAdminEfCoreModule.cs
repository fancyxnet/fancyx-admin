using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;

using Microsoft.Extensions.Configuration;

namespace Fancyx.Admin.EfCore
{
    [DependsOn(
        typeof(FancyxEfCoreModule)
        )]
    public class FancyxAdminEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<FancyxDbContext>(context.Configuration.GetConnectionString("Default")!);
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}