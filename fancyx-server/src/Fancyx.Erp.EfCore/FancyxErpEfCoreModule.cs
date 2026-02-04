using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Cracker.EfCore;
using Fancyx.Shared.EfCore;
using Microsoft.Extensions.Configuration;

namespace Fancyx.Erp.EfCore
{
    [DependsOn(
    typeof(FancyxSharedEfCoreModule)
    )]
    public class FancyxErpEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<FancyxErpDbContext>(options =>
            {
                options.ConnectionString = context.Configuration.GetConnectionString("Default")!;
            });
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}
