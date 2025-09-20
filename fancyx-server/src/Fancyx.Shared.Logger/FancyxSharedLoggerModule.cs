using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EfCore;
using Fancyx.EventBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Logger
{
    [DependsOn(
        typeof(FancyxEventBusModule)
        )]
    public class FancyxSharedLoggerModule : ModuleBase
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