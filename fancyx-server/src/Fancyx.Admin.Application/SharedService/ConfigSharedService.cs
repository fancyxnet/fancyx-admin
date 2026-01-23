using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Cache;
using Fancyx.Core.AutoInject;
using Fancyx.EfCore;
using Fancyx.Shared.Keys;

using StackExchange.Redis;

namespace Fancyx.Admin.Application.SharedService
{
    [DependencyInject(AsSelf = true)]
    public class ConfigSharedService
    {
        private readonly IRepository<Config> _configRepository;
        private readonly ICacheClient _cache;

        public ConfigSharedService(IRepository<Config> configRepository, ICacheClient cache)
        {
            _configRepository = configRepository;
            _cache = cache;
        }

        public async Task<string?> GetAsync(string key)
        {
            if (await _cache.HashExistsAsync(SystemCacheKey.SystemConfig, key))
            {
                return (string?)await _cache.HashGetAsync(SystemCacheKey.SystemConfig, key);
            }

            string? value = await _configRepository.Where(x => x.Key.ToLower() == key.ToLower()).ToOneAsync(e => e.Value);
            if (value != null)
            {
                await _cache.HashSetAsync(SystemCacheKey.SystemConfig, key, value);
            }
            return value;
        }

        public async Task<Dictionary<string, string>> GetGroupAsync(string group)
        {
            string key = SystemCacheKey.SystemConfigGroup(group);
            if (await _cache.KeyExistsAsync(key))
            {
                var groups = await _cache.HashGetAllAsync(key);
                var map = new Dictionary<string, string>();
                foreach (var item in groups)
                {
                    map.Add((string)item.Name!, item.Value!);
                }
            }

            var groupKeys = _configRepository.Where(x => !string.IsNullOrEmpty(x.GroupKey) && x.GroupKey.ToLower() == group.ToLower())
                .ToDictionary(k => k.Key, v => v.Value);
            if (groupKeys.Count > 0)
            {
                await _cache.HashSetAsync(key, groupKeys.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
                return groupKeys;
            }
            return groupKeys;
        }

        public void ClearCache(string key)
        {
            _cache.HashDelete(SystemCacheKey.SystemConfig, key);
        }

        public void ClearGroupCache(string group)
        {
            string key = SystemCacheKey.SystemConfigGroup(group);
            _cache.KeyDelete(key);
        }
    }
}