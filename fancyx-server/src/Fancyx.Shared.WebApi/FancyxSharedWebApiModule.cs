using Fancyx.Cache;
using Fancyx.Core;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Shared.WebApi.Filters;
using Fancyx.Shared.WebApi.Handlers;
using Fancyx.SnowflakeId;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Fancyx.Shared.WebApi
{
    [DependsOn(
        typeof(FancyxCacheModule)
        )]
    public class FancyxSharedWebApiModule : ModuleBase
    {
        public override int Order => 99;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;
            context.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<HttpRequestValidationFilter>();
                options.Filters.Add<AppGlobalExceptionFilter>(1);
            });
            context.Services.AddSingleton<PermissionCacheHandler>();
            context.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CommonAuthorizationMiddlewareResultHandler>();
            context.Services.AddJwt(configuration);

            IdGenerater.Init(short.Parse(context.Configuration["Snowflake:WorkerId"]!), short.Parse(context.Configuration["Snowflake:DataCenterId"]!));
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
        }
    }
}