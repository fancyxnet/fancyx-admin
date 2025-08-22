using Fancyx.Admin.Entities.System;
using Fancyx.Core.Interfaces;
using Fancyx.Repository;
using Fancyx.Shared.Keys;

using StackExchange.Redis;

namespace Fancyx.Admin.SharedService
{
    public class ConfigSharedService : IScopedDependency
    {
        private readonly IRepository<ConfigDO> _configRepository;
        private readonly IDatabase _redisClient;

        public ConfigSharedService(IRepository<ConfigDO> configRepository, IDatabase redisClient)
        {
            _configRepository = configRepository;
            _redisClient = redisClient;
        }

        public async Task<string?> GetAsync(string key)
        {
            if (await _redisClient.HashExistsAsync(SystemCacheKey.SystemConfig, key))
            {
                return (string?)await _redisClient.HashGetAsync(SystemCacheKey.SystemConfig, key);
            }

            string? value = await _configRepository.Select.Where(x => x.Key.ToLower() == key.ToLower()).ToOneAsync(e => e.Value);
            if (value != null)
            {
                await _redisClient.HashSetAsync(SystemCacheKey.SystemConfig, key, value);
            }
            return value;
        }

        public async Task<Dictionary<string, string>> GetGroupAsync(string group)
        {
            string key = SystemCacheKey.SystemConfigGroup(group);
            if (await _redisClient.KeyExistsAsync(key))
            {
                var groups = await _redisClient.HashGetAllAsync(key);
                var map = new Dictionary<string, string>();
                foreach (var item in groups)
                {
                    map.Add((string)item.Name!, item.Value!);
                }
            }

            var groupKeys = _configRepository.Select.Where(x => !string.IsNullOrEmpty(x.GroupKey) && x.GroupKey.ToLower() == group.ToLower())
                .ToDictionary(k => k.Key, v => v.Value);
            if (groupKeys.Count > 0)
            {
                await _redisClient.HashSetAsync(key, groupKeys.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
                return groupKeys;
            }
            return groupKeys;
        }

        public void ClearCache(string key)
        {
            _redisClient.HashDelete(SystemCacheKey.SystemConfig, key);
        }

        public void ClearGroupCache(string group)
        {
            string key = SystemCacheKey.SystemConfigGroup(group);
            _redisClient.KeyDelete(key);
        }
    }
}