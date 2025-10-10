using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

namespace Fancyx.Orleans
{
    public static class OrleansExtension
    {
        public static void UseOrleansWithRedis(this ConfigureHostBuilder host, IConfiguration configuration)
        {
            var orleansConfig = configuration.GetRequiredSection("Orleans").Get<OrleansConfigOption>()!;
            host.UseOrleans((context, silo) =>
            {
                silo.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = orleansConfig.ClusterId;
                    options.ServiceId = orleansConfig.ServiceId;
                }).UseRedisClustering(opt =>
                {
                    opt.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                    {
                        EndPoints = { orleansConfig.RedisEndPoints },
                        AllowAdmin = true,
                        AbortOnConnectFail = false
                    };
                }).AddRedisGrainStorage(orleansConfig.StorageName, opt =>
                {
                    opt.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                    {
                        EndPoints = { orleansConfig.RedisEndPoints },
                        AllowAdmin = true,
                        AbortOnConnectFail = false
                    };
                    opt.GetStorageKey = (grainType, grainId) => $"{orleansConfig.ClusterId}/{orleansConfig.ServiceId}/{grainType}/{grainId.Key}";
                });
            });
        }
    }
}