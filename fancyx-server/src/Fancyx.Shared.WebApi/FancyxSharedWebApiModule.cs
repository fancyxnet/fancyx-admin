using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EventBus;
using Fancyx.Logger;
using Fancyx.Redis;
using Fancyx.Shared.WebApi.Filters;
using Fancyx.Shared.WebApi.Handlers;
using Fancyx.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Shared.WebApi
{
    [DependsOn(
        typeof(FancyxRedisModule),
        typeof(FancyxEventBusModule),
        typeof(FancyxSharedLoggerModule)
        )]
    public class FancyxSharedWebApiModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<HttpRequestValidationFilter>();
                options.Filters.Add<AppGlobalExceptionFilter>(1);
            });
            context.Services.AddSingleton<PermissionCacheHandler>();
            context.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CommonAuthorizationMiddlewareResultHandler>();

            SnowflakeHelper.Init(short.Parse(context.Configuration["Snowflake:WorkerId"]!), short.Parse(context.Configuration["Snowflake:DataCenterId"]!));
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}
