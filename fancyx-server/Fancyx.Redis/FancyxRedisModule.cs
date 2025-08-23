using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;

using Microsoft.Extensions.DependencyInjection;

using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

using StackExchange.Redis;

namespace Fancyx.Redis
{
    public class FancyxRedisModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();

            //StackExchange.Redis
            context.Services.AddSingleton<IDatabase>(r =>
            {
                var connection = ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!);
                return connection.GetDatabase(0);
            });
            context.Services.AddSingleton<IHybridCache, HybridCache>();

            //RedLock
            var multiplexers = new List<RedLockMultiplexer>
            {
                ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!)
            };
            context.Services.AddSingleton<RedLockFactory>(RedLockFactory.Create(multiplexers));
        }
    }
}