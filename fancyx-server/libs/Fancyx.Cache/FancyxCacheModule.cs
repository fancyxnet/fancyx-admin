using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Core.Interfaces;
using Microsoft.AspNetCore.Http;
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
            context.Services.AddScoped<ICacheClient, CacheClient>(p =>
            {
                var conn = p.GetRequiredService<IConnectionMultiplexer>();

                var ctx = p.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
                if (ctx != null)
                {
                    var tenant = ctx.RequestServices.GetRequiredService<ICurrentTenant>();
                    if (tenant != null && !string.IsNullOrEmpty(tenant.TenantId))
                    {
                        return new CacheClient(conn, $"tenant:{tenant.TenantId}:");
                    }
                }
                return new CacheClient(conn, "");
            });

            var multiplexers = new List<RedLockMultiplexer>
            {
                ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!)
            };
            context.Services.AddSingleton(RedLockFactory.Create(multiplexers));
        }
    }
}