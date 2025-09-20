using Fancyx.Consul;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Shared.WebApi.Micro
{
    public static class MicroSetup
    {
        public static void AddMicroService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.ListenLocalhost(int.Parse(configuration["Consul:HttpPort"]!), listenOptions => listenOptions.Protocols = HttpProtocols.Http1);
                if (!string.IsNullOrEmpty(configuration["Consul:GrpcPort"]))
                {
                    options.ListenLocalhost(int.Parse(configuration["Consul:GrpcPort"]!), listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
                }
            });
            if (configuration["Services:Mode"] == "Consul")
            {
                services.AddConsulSetup(configuration);
            }
        }

        public static void UseMicroDiscovery(this WebApplication app)
        {
            var configuration = app.Services.GetRequiredService<IConfiguration>()!;
            if (configuration["Services:Mode"] == "Consul")
            {
                ConsulRegistration.Register(app.Services);
                app.MapHealthChecks(ConsulConstant.ConsulHealthUrl);
            }
        }
    }
}
