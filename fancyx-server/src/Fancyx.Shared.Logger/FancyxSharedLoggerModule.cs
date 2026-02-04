using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Cracker.EventBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Shared.Logger
{
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
            context.Services.AddEventBus(new EventBusOptions
            {
                RedisConnection = context.Configuration[""]
            });
        }
    }
}