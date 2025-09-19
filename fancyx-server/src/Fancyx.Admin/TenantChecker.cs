using Fancyx.Core.Interfaces;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.System;
using Fancyx.Shared.Keys;

using StackExchange.Redis;

namespace Fancyx.Admin
{
    public class TenantChecker : ITenantChecker, ISingletonDependency
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDatabase _redisClient;

        public TenantChecker(IRepository<Tenant> tenantRepository, IDatabase redisClient)
        {
            _tenantRepository = tenantRepository;
            _redisClient = redisClient;
        }

        public async Task<bool> ExistTenantAsync(string tenantId)
        {
            if (await _redisClient.KeyExistsAsync(SystemCacheKey.AllTenant)) return await _redisClient.HashExistsAsync(SystemCacheKey.AllTenant, tenantId);

            var tenants = await _tenantRepository.GetQueryable().SelectToListAsync(x => new { x.TenantId, x.Name });
            var map = tenants.ToDictionary(k => k.TenantId, v => v.Name);
            if (map.Count == 0) return false;

            await _redisClient.HashSetAsync(SystemCacheKey.AllTenant, map.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
            return map.ContainsKey(tenantId);
        }
    }
}