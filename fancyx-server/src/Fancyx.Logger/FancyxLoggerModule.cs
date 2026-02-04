using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Cracker.EfCore;
using Fancyx.Shared.Logger;

namespace Fancyx.Logger
{
    [DependsOn(
        typeof(FancyxSharedLoggerModule)
    )]
    public class FancyxLoggerModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddEfCore<LoggerDbContext>(options =>
            {
                options.ConnectionString = context.Configuration.GetConnectionString("Default")!;
            });
        }
    }
}
