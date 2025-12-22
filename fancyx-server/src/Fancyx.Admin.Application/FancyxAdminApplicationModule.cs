using Coravel;

using Fancyx.Admin.Application.Grpc;
using Fancyx.Admin.Application.Jobs;
using Fancyx.Admin.Application.WebSockets;
using Fancyx.Admin.EfCore;
using Fancyx.Cache;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EventBus;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace Fancyx.Admin.Application
{
    [DependsOn(
         typeof(FancyxCacheModule),
         typeof(FancyxEventBusModule),
         typeof(FancyxSharedLoggerModule),
         typeof(FancyxAdminEfCoreModule)
        )]
    public class FancyxAdminApplicationModule : ModuleBase
    {
        public override int Order => 99;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScheduler();

            var channel = Channel.CreateUnbounded<NotificationMessage>();
            context.Services.AddSingleton(sp =>
            {
                return channel.Writer;
            });
            context.Services.AddSingleton(sp =>
            {
                return channel.Reader;
            });
            context.Services.AddSingleton<WebSocketConnectionManager>();
            context.Services.AddHostedService<NotificationBgService>();
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.ApplicationServices.UseScheduler(sch =>
            {
                sch.Schedule<NotificationJob>().EveryMinute().PreventOverlapping(nameof(NotificationJob));
            });
            context.Endpoint.MapGrpcService<TestGrpcServiceHandler>();
            context.Endpoint.MapGrpcService<DictGrpcServiceHandler>();
            context.Endpoint.MapGrpcService<AuthGrpcServiceHandler>();

            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
