using Microsoft.Extensions.Caching.Memory;

using StackExchange.Redis;

using System.Text.Json;

namespace Fancyx.Redis
{
    internal class HybridCache : IHybridCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDatabase _redisClient;
        private readonly TimeSpan _defaultExpiration;

        public HybridCache(
            IMemoryCache memoryCache,
            IDatabase redisClient,
            TimeSpan? defaultExpiration = null)
        {
            _memoryCache = memoryCache;
            _redisClient = redisClient;
            _defaultExpiration = defaultExpiration ?? TimeSpan.FromDays(7);
        }

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? expiration = null,
            HybridCacheMode mode = HybridCacheMode.Both)
        {
            var value = await GetAsync<T>(key, mode);
            if (value != null)
            {
                return value;
            }

            value = await factory();
            await SetAsync(key, value, expiration, mode);
            return value;
        }

        public async Task<T?> GetAsync<T>(string key, HybridCacheMode mode = HybridCacheMode.Both)
        {
            // 先尝试从内存缓存获取
            if (mode == HybridCacheMode.MemoryOnly || mode == HybridCacheMode.Both)
            {
                if (_memoryCache.TryGetValue(key, out T? memoryValue))
                {
                    return memoryValue;
                }
            }

            // 如果内存缓存没有，尝试从Redis获取
            if (mode == HybridCacheMode.RedisOnly || mode == HybridCacheMode.Both)
            {
                var redisValue = (string?)await _redisClient.StringGetAsync(key);
                if (redisValue != null)
                {
                    var value = JsonSerializer.Deserialize<T>(redisValue);

                    // 如果模式是Both，将Redis的值写入内存缓存
                    if (mode == HybridCacheMode.Both)
                    {
                        var expiration = _defaultExpiration;
                        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = expiration
                        });
                    }

                    return value;
                }
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, HybridCacheMode mode = HybridCacheMode.Both)
        {
            var actualExpiration = expiration ?? _defaultExpiration;

            // 存储顺序：先Redis，再内存
            if (mode == HybridCacheMode.RedisOnly || mode == HybridCacheMode.Both)
            {
                var redisValue = JsonSerializer.Serialize(value);
                await _redisClient.StringSetAsync(key, redisValue, actualExpiration);
            }

            if (mode == HybridCacheMode.MemoryOnly || mode == HybridCacheMode.Both)
            {
                _memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = actualExpiration
                });
            }
        }

        public async Task RemoveAsync(string key, HybridCacheMode mode = HybridCacheMode.Both)
        {
            // 移除顺序：先内存，再Redis
            if (mode == HybridCacheMode.MemoryOnly || mode == HybridCacheMode.Both)
            {
                _memoryCache.Remove(key);
            }

            if (mode == HybridCacheMode.RedisOnly || mode == HybridCacheMode.Both)
            {
                await _redisClient.KeyDeleteAsync(key);
            }
        }

        public async Task RemoveByPatternAsync(string pattern, HybridCacheMode mode = HybridCacheMode.Both)
        {
            var matches = await this.KeyPatternAsync(pattern, mode);

            foreach (var key in matches)
            {
                await RemoveAsync(key, mode);
            }
        }

        public async Task<List<string>> KeyPatternAsync(string pattern, HybridCacheMode mode = HybridCacheMode.Both)
        {
            return (await _redisClient.KeyPatternAsync(pattern))?.ToList() ?? [];
        }

        public async Task<bool> ExistsAsync(string key, HybridCacheMode mode = HybridCacheMode.Both)
        {
            if (mode == HybridCacheMode.MemoryOnly || mode == HybridCacheMode.Both)
            {
                if (_memoryCache.TryGetValue(key, out _))
                {
                    return true;
                }
            }

            if (mode == HybridCacheMode.RedisOnly || mode == HybridCacheMode.Both)
            {
                return await _redisClient.KeyExistsAsync(key);
            }

            return false;
        }
    }
}