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
            configurationManager.AddConsul($"{configuration["Consul:NodeName"]}/appsettings.json", options =>
            {
                options.ConsulConfigurationOptions = ConfigureConsul;
                options.Optional = true;
                options.ReloadOnChange = true;
                options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
            });
            services.AddMemoryCache();
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(ConfigureConsul));
            services.AddSingleton<ConsulHelper>();
            services.AddSingleton<ConsulDiscoverHttpHandler>();
            services.AddHealthChecks().AddCheck<ServiceHealthCheck>("service_health");
            return;

            void ConfigureConsul(ConsulClientConfiguration cco)
            {
                cco.Address = new Uri(configuration["Consul:Host"]!);
                cco.Token = configuration["Consul:Token"];
            }
        }
    }
}
