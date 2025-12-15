using Consul;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fancyx.Consul
{
    public class ConsulRegistration
    {
        public static void Register(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var consulClient = serviceProvider.GetRequiredService<IConsulClient>();
            var hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

            var hostAddress = configuration["Consul:NodeAddress"];
            if (string.IsNullOrEmpty(hostAddress))
            {
                hostAddress = Environment.GetEnvironmentVariable("HOST_ADDRESS");
            }
            if (string.IsNullOrEmpty(hostAddress))
            {
                hostAddress = "localhost";
            }
            var hostPortString = configuration["Consul:HttpPort"]!;
            var hostPort = int.Parse(hostPortString);
            var serverUrl = $"http://{hostAddress}:{hostPort}";

            var nodeName = configuration["Consul:NodeName"]!;

            var registration = new AgentServiceRegistration
            {
                ID = $"{nodeName}-{hostAddress.Replace('.','_')}-{hostPort}",
                Name = nodeName,
                Address = hostAddress,
                Port = hostPort,
                Meta = new Dictionary<string, string>
                {
                    { "HttpPort", hostPortString },
                    { "GrpcPort", configuration["Consul:GrpcPort"]! },
                },
                Check = new AgentServiceCheck
                {
                    HTTP = new Uri(new Uri(serverUrl), ConsulConstant.ConsulHealthUrl).ToString(),
                    Interval = TimeSpan.FromSeconds(30),
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30),
                    TLSSkipVerify = true
                }
            };

            hostApplicationLifetime.ApplicationStarted.Register(async () => await consulClient.Agent.ServiceRegister(registration));
            hostApplicationLifetime.ApplicationStopping.Register(async () => await consulClient.Agent.ServiceDeregister(registration.ID));
        }
    }
}
