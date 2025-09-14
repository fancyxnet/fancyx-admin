using StackExchange.Redis;

namespace Fancyx.Redis
{
    public static class RedisExtension
    {
        public static async Task<string[]?> KeyPatternAsync(this IDatabase database, string pattern, int count = 100)
        {
            var redisResult = await database.ScriptEvaluateAsync(@"
                local pattern = ARGV[1]
                local count = tonumber(ARGV[2])
                local cursor = '0'
                local allKeys = {}
                
                repeat
                    local res = redis.call('SCAN', cursor, 'MATCH', pattern, 'COUNT', count)
                    cursor = res[1]
                    local foundKeys = res[2]
                    for i, key in ipairs(foundKeys) do
                        table.insert(allKeys, key)
                    end
                until cursor == '0'
                
                return allKeys",
                 values: new RedisValue[] { pattern, count }
             );

            if (redisResult.IsNull)
                return [];

            return (string[])redisResult!;
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