using Calzolari.Grpc.AspNetCore.Validation;

using Fancyx.Consul;
using Fancyx.Shared.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fancyx.Shared.WebApi.Micro
{
    public static class MicroSetup
    {
        public static bool EnabledGrpc { get; private set; }
        public static bool EnabledConsul { get; private set; }

        public static void AddMicroService(this IServiceCollection services, IConfiguration configuration)
        {
            EnabledGrpc = int.TryParse(configuration["Consul:GrpcPort"], out var grpcPort) && grpcPort > 0;
            EnabledConsul = configuration["Services:Mode"] == "Consul";
            services.Configure<KestrelServerOptions>(options =>
            {
                options.ListenLocalhost(int.Parse(configuration["Consul:HttpPort"]!), listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                if (EnabledGrpc)
                {
                    options.ListenLocalhost(grpcPort, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
                }
            });
            services.Configure<MicroServiceOption>(configuration.GetSection("Services"));
            if (EnabledConsul)
            {
                services.AddConsulSetup(configuration);
            }
            if (EnabledGrpc)
            {
                services.AddGrpc(options =>
                {
                    options.EnableMessageValidation();
                });
                services.AddGrpcReflection();
                services.AddGrpcValidation();
            }
        }

        public static void UseMicroDiscovery(this WebApplication app)
        {
            if (EnabledConsul)
            {
                ConsulRegistration.Register(app.Services);
                app.MapHealthChecks(ConsulConstant.ConsulHealthUrl);
            }
            if (EnabledGrpc && app.Environment.IsDevelopment())
            {
                app.MapGrpcReflectionService().AllowAnonymous();
            }
        }
    }
}
