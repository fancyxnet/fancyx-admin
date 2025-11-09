using Coravel;

using Fancyx.Admin.Application.Grpc;
using Fancyx.Admin.Application.Jobs;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.EventBus;
using Fancyx.Redis;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

using MQTTnet.AspNetCore;

namespace Fancyx.Admin.Application
{
    [DependsOn(
         typeof(FancyxRedisModule),
         typeof(FancyxEventBusModule),
         typeof(FancyxSharedLoggerModule),
         typeof(FancyxAdminEfCoreModule)
        )]
    public class FancyxAdminApplicationModule : ModuleBase
    {
        public override int Order => 99;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = context.Configuration;

            services.Configure<KestrelServerOptions>(options =>
            {
                options.ListenAnyIP(port: int.Parse(configuration["Mqtt:Port"]!), l => l.UseMqtt());
            });
            services.AddHostedMqttServer(optionsBuilder =>
            {
                optionsBuilder.WithDefaultEndpoint();
            });
            services.AddMqttConnectionHandler();
            services.AddScheduler();
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            context.Endpoint.MapConnectionHandler<MqttConnectionHandler>("/mqtt", httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            app.UseMqttServer(server =>
            {
                var mqttService = context.ServiceProvider.GetRequiredService<MqttSharedService>();
                server.ValidatingConnectionAsync += mqttService.ValidatingConnectionAsync;
            });

            app.ApplicationServices.UseScheduler(sch =>
            {
                sch.Schedule<NotificationJob>().EveryMinute().PreventOverlapping(nameof(NotificationJob));
            });
            context.Endpoint.MapGrpcService<TestGrpcServiceHandler>();
            context.Endpoint.MapGrpcService<DictGrpcServiceHandler>();
            context.Endpoint.MapGrpcService<AuthGrpcServiceHandler>();
        }
    }
}
