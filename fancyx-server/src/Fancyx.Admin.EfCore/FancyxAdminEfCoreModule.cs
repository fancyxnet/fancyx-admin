using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Cracker.EfCore;
using Fancyx.Shared.EfCore;
using Microsoft.Extensions.Configuration;

namespace Fancyx.Admin.EfCore
{
    [DependsOn(
        typeof(FancyxSharedEfCoreModule)
        )]
    public class FancyxAdminEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<FancyxDbContext>(options =>
            {
                options.ConnectionString = context.Configuration.GetConnectionString("Default")!;
                options.EnabledMultiTenancy = true;
            });
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}