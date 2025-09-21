using Autofac.Core;

using Fancyx.Consul.Discover;
using Fancyx.Shared.Models;

using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Refit;

namespace Fancyx.Shared.WebApi.Micro
{
    public class RemoteClient
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public RemoteClient(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public RemoteClient AddHttp<THttpClient>(string serviceName) where THttpClient : class
        {
            var clientBuilder = _services.AddRefitClient<THttpClient>().SetHandlerLifetime(TimeSpan.FromMinutes(2));
            var options = _configuration.GetRequiredSection("Services").Get<MicroServiceOption>() ?? throw new ArgumentNullException("options", "appsettings.Services配置不存在");
            switch (options.Mode)
            {
                case "Direct":
                    var addressNode = options.Address?.FirstOrDefault(x => x.Name == serviceName) ?? throw new ArgumentNullException("addressNode", $"直连服务{serviceName}的Http配置不存在");
                    clientBuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.Http!));
                    break;
                case "Consul":
                    clientBuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri($"http://{serviceName}"))
                    .AddHttpMessageHandler<ConsulDiscoverHttpHandler>();
                    break;
                default:
                    throw new NotSupportedException($"不支持的服务注册与发现模式 => {options.Mode}");
            }
            return this;
        }

        public RemoteClient AddGrpc<TGrpcClient>(string serviceName) where TGrpcClient : ClientBase<TGrpcClient>
        {
            var clientBuilder = _services.AddGrpcClient<TGrpcClient>()
             .ConfigureChannel(options =>
             {
                 options.Credentials = ChannelCredentials.Insecure;
                 options.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
                 options.UnsafeUseInsecureChannelCallCredentials = true;
             });
            var options = _configuration.GetRequiredSection("Services").Get<MicroServiceOption>() ?? throw new ArgumentNullException("options", "appsettings.Services配置不存在");
            switch (options.Mode)
            {
                case "Direct":
                    var addressNode = options.Address?.FirstOrDefault(x => x.Name == serviceName) ?? throw new ArgumentNullException("addressNode", $"直连服务{serviceName}的Http配置不存在");
                    clientBuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.Grpc!));
                    break;
                case "Consul":
                    _services.TryAddSingleton<ResolverFactory, ConsulGrpcResolverFactory>();
                    clientBuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri($"consul://{serviceName}"));
                    break;
                default:
                    throw new NotSupportedException($"不支持的服务注册与发现模式 => {options.Mode}");
            }
            return this;
        }
    }

    public static class RemoteClientSetup
    {
        public static void AddRemoteClient(this IServiceCollection services, IConfiguration configuration, Action<RemoteClient> action)
        {
            var client = new RemoteClient(services, configuration);
            action(client);
        }
    }
}
