using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Microsoft.Extensions.DependencyInjection;

using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

using StackExchange.Redis;

namespace Fancyx.Cache
{
    public class FancyxCacheModule : ModuleBase
    {
        public override void Configure(ApplicationInitializationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();

            context.Services.AddSingleton<IConnectionMultiplexer>(r =>
            {
                return ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!);
            });
            context.Services.AddScoped<ICacheClient, CacheClient>();

            var multiplexers = new List<RedLockMultiplexer>
            {
                ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!)
            };
            context.Services.AddSingleton(RedLockFactory.Create(multiplexers));
        }
    }
}