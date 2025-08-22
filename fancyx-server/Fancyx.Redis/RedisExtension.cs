using StackExchange.Redis;

namespace Fancyx.Redis
{
    public static class RedisExtension
    {
        public static async Task<string[]?> KeyPatternAsync(this IDatabase database, string pattern)
        {
            var redisResult = await database.ScriptEvaluateAsync("local res = redis.call('KEYS', @keypattern) return res");
            return ((string[]?)redisResult);
        }

        public static async Task KeyDeleteByPatternAsync(this IDatabase database, string pattern)
        {
            var keys = await database.KeyPatternAsync(pattern);
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    await database.KeyDeleteAsync(key);
                }
            }
        }
    }
}