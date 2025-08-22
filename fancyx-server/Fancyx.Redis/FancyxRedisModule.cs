using Fancyx.Core.Authorization;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;

using Microsoft.Extensions.DependencyInjection;

using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;

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
            var connection = ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!);
            context.Services.AddSingleton<ConnectionMultiplexer>(r => connection);
            context.Services.AddSingleton<IDatabase>(r =>
            {
                var db = connection.GetDatabase(0);
                if (!string.IsNullOrEmpty(TenantManager.Current))
                {
                    return db.WithKeyPrefix(TenantManager.Current);
                }
                return db;
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