using Calzolari.Grpc.AspNetCore.Validation;

using Fancyx.Consul;
using Fancyx.Internal.Grpc.System;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fancyx.Shared.WebApi.Micro
{
    public static class MicroSetup
    {
        public static bool EnabledConsul { get; private set; }

        public static void AddMicroService(this IServiceCollection services, IConfiguration configuration)
        {
            int grpcPort = int.Parse(configuration["Consul:GrpcPort"]!);
            EnabledConsul = configuration["Services:Mode"] == "Consul";
            services.Configure<KestrelServerOptions>(options =>
            {
                options.ListenLocalhost(int.Parse(configuration["Consul:HttpPort"]!), listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                options.ListenLocalhost(grpcPort, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
            });
            services.Configure<MicroServiceOption>(configuration.GetSection("Services"));
            if (EnabledConsul)
            {
                services.AddConsulSetup(configuration);
            }
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
            });
            services.AddGrpcReflection();
            services.AddGrpcValidation();
            services.AddSingleton<GrpcHeaderInterceptor>();
            services.AddRemoteClient(configuration, options => options.AddGrpc<Auth.AuthClient>(MicroServiceConsts.AdminApi));
        }

        public static void UseMicroDiscovery(this WebApplication app)
        {
            if (EnabledConsul)
            {
                ConsulRegistration.Register(app.Services);
                app.Map(ConsulConstant.ConsulHealthUrl, () =>
                {
                    // TODO:
                    return Results.Ok("ok");
                });
            }
            if (app.Environment.IsDevelopment())
            {
                app.MapGrpcReflectionService().AllowAnonymous();
            }
        }
    }
}
