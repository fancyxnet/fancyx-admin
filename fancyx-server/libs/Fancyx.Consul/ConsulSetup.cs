using Consul;

using Fancyx.Consul.Discover;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Winton.Extensions.Configuration.Consul;

namespace Fancyx.Consul
{
    public static class ConsulSetup
    {
        public static void AddConsulSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationManager = (ConfigurationManager)configuration;
            configurationManager.AddConsulConfiguration($"{configuration["Consul:NodeName"]}/appsettings.json");
            services.AddMemoryCache();
            services.AddSingleton<IConsulClient>(sp => new ConsulClient((cco) =>
            {
                cco.Address = new Uri(configuration["Consul:Host"]!);
                cco.Token = configuration["Consul:Token"];
            }));
            services.AddSingleton<ConsulHelper>();
            services.AddSingleton<ConsulDiscoverHttpHandler>();
            return;
        }

        public static void AddConsulConfiguration(this ConfigurationManager configuration, string key)
        {
            configuration.AddConsul(key, options =>
            {
                options.ConsulConfigurationOptions = (cco) =>
                {
                    cco.Address = new Uri(configuration["Consul:Host"]!);
                    cco.Token = configuration["Consul:Token"];
                };
                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
            });
        }
    }
}
