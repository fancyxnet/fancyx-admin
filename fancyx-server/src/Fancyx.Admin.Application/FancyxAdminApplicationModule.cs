using Fancyx.Admin.Application.Grpc;
using Fancyx.Admin.Application.Jobs;
using Fancyx.Admin.Application.WebSockets;
using Fancyx.Admin.EfCore;
using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using Cracker.Scheduler;
using Microsoft.Extensions.Configuration;

namespace Fancyx.Admin.Application
{
    [DependsOn(
         typeof(FancyxSharedLoggerModule),
         typeof(FancyxAdminEfCoreModule)
        )]
    public class FancyxAdminApplicationModule : ModuleBase
    {
        public override int Order => 99;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScheduler(o =>
            {
                o.AutoCreatedTable = true;
                o.ConnectionString = context.Configuration.GetConnectionString("Default")!;
                o.Configure = s =>
                {
                    s.AddJob<NotificationJob>("0 0/1 * * * ?", nameof(NotificationJob));
                };
            });

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
            context.Application.MapGrpcService<TestGrpcServiceHandler>();
            context.Application.MapGrpcService<DictGrpcServiceHandler>();
            context.Application.MapGrpcService<AuthGrpcServiceHandler>();

            context.Application.UseWebSockets();
            context.Application.UseMiddleware<WebSocketMiddleware>();
            context.Application.UseSchedulerDashboard();
        }
    }
}
