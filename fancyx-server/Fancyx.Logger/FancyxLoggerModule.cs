using Fancyx.Cap;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Logger
{
    [DependsOn(
        typeof(FancyxCapModule),
        typeof(FancyxRepositoryModule)
        )]
    public class FancyxLoggerModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<ExceptionLogFilter>(99);
            });
        }
    }
}