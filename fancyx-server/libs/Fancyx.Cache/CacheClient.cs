using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;
using System.Net;
using System.Text.Json;

namespace Fancyx.Cache
{
    internal class CacheClient : ICacheClient
    {
        private IDatabase _redis;
        private string? _prefix;

        public CacheClient(IConnectionMultiplexer connection)
        {
            _redis = connection.GetDatabase();
        }

        #region custom

        public void WithKeyPrefix(string prefix)
        {
            _prefix = prefix;
            _redis = _redis.WithKeyPrefix(prefix);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expire = null)
        {
            await _redis.StringSetAsync(key, JsonSerializer.Serialize(value), expire);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);

            if (value.IsNull || !value.HasValue)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task<string[]?> KeyPatternAsync(string pattern, int count = 100)
        {
            if (_prefix != null && !pattern.StartsWith(_prefix))
            {
                pattern = $"{_prefix}{pattern}";
            }
            var redisResult = await _redis.ScriptEvaluateAsync(@"
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
                return Array.Empty<string>();

            return (string[])redisResult!;
        }

        public async Task KeyDeleteByPatternAsync(string pattern)
        {
            var keys = await this.KeyPatternAsync(pattern);
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    await this.KeyDeleteAsync(key);
                }
            }
        }

        #endregion

        public int Database => _redis.Database;

        public IConnectionMultiplexer Multiplexer => _redis.Multiplexer;

        public IBatch CreateBatch(object? asyncState = null) => _redis.CreateBatch(asyncState);

        public ITransaction CreateTransaction(object? asyncState = null) => _redis.CreateTransaction(asyncState);

        public RedisValue DebugObject(RedisKey key, CommandFlags flags = CommandFlags.None) => _redis.DebugObject(key, flags);

        public Task<RedisValue> DebugObjectAsync(RedisKey key, CommandFlags flags = CommandFlags.None) => _redis.DebugObjectAsync(key, flags);

        public RedisResult Execute(string command, params object[] args) => _redis.Execute(command, args);

        public RedisResult Execute(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None) => _redis.Execute(command, args, flags);

        public Task<RedisResult> ExecuteAsync(string command, params object[] args) => _redis.ExecuteAsync(command, args);

        public Task<RedisResult> ExecuteAsync(string command, ICollection<object>? args, CommandFlags flags = CommandFlags.None) => _redis.ExecuteAsync(command, args, flags);

        public bool GeoAdd(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAdd(key, longitude, latitude, member, flags);

        public bool GeoAdd(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAdd(key, value, flags);

        public long GeoAdd(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAdd(key, values, flags);

        public Task<bool> GeoAddAsync(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAddAsync(key, longitude, latitude, member, flags);

        public Task<bool> GeoAddAsync(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAddAsync(key, value, flags);

        public Task<long> GeoAddAsync(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
            => _redis.GeoAddAsync(key, values, flags);

        public double? GeoDistance(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
            => _redis.GeoDistance(key, member1, member2, unit, flags);

        public Task<double?> GeoDistanceAsync(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
            => _redis.GeoDistanceAsync(key, member1, member2, unit, flags);

        public string?[] GeoHash(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.GeoHash(key, members, flags);

        public string? GeoHash(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoHash(key, member, flags);

        public Task<string?[]> GeoHashAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.GeoHashAsync(key, members, flags);

        public Task<string?> GeoHashAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoHashAsync(key, member, flags);

        public GeoPosition?[] GeoPosition(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.GeoPosition(key, members, flags);

        public GeoPosition? GeoPosition(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoPosition(key, member, flags);

        public Task<GeoPosition?[]> GeoPositionAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.GeoPositionAsync(key, members, flags);

        public Task<GeoPosition?> GeoPositionAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoPositionAsync(key, member, flags);

        public GeoRadiusResult[] GeoRadius(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRadius(key, member, radius, unit, count, order, options, flags);

        public GeoRadiusResult[] GeoRadius(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRadius(key, longitude, latitude, radius, unit, count, order, options, flags);

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRadiusAsync(key, member, radius, unit, count, order, options, flags);

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRadiusAsync(key, longitude, latitude, radius, unit, count, order, options, flags);

        public bool GeoRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRemove(key, member, flags);

        public Task<bool> GeoRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.GeoRemoveAsync(key, member, flags);

        public GeoRadiusResult[] GeoSearch(RedisKey key, RedisValue member, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearch(key, member, shape, count, demandClosest, order, options, flags);

        public GeoRadiusResult[] GeoSearch(RedisKey key, double longitude, double latitude, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearch(key, longitude, latitude, shape, count, demandClosest, order, options, flags);

        public long GeoSearchAndStore(RedisKey sourceKey, RedisKey destinationKey, RedisValue member, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, bool storeDistances = false, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAndStore(sourceKey, destinationKey, member, shape, count, demandClosest, order, storeDistances, flags);

        public long GeoSearchAndStore(RedisKey sourceKey, RedisKey destinationKey, double longitude, double latitude, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, bool storeDistances = false, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAndStore(sourceKey, destinationKey, longitude, latitude, shape, count, demandClosest, order, storeDistances, flags);

        public Task<long> GeoSearchAndStoreAsync(RedisKey sourceKey, RedisKey destinationKey, RedisValue member, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, bool storeDistances = false, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAndStoreAsync(sourceKey, destinationKey, member, shape, count, demandClosest, order, storeDistances, flags);

        public Task<long> GeoSearchAndStoreAsync(RedisKey sourceKey, RedisKey destinationKey, double longitude, double latitude, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, bool storeDistances = false, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAndStoreAsync(sourceKey, destinationKey, longitude, latitude, shape, count, demandClosest, order, storeDistances, flags);

        public Task<GeoRadiusResult[]> GeoSearchAsync(RedisKey key, RedisValue member, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAsync(key, member, shape, count, demandClosest, order, options, flags);

        public Task<GeoRadiusResult[]> GeoSearchAsync(RedisKey key, double longitude, double latitude, GeoSearchShape shape, int count = -1, bool demandClosest = true, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
            => _redis.GeoSearchAsync(key, longitude, latitude, shape, count, demandClosest, order, options, flags);

        public long HashDecrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.HashDecrement(key, hashField, value, flags);

        public double HashDecrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
            => _redis.HashDecrement(key, hashField, value, flags);

        public Task<long> HashDecrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.HashDecrementAsync(key, hashField, value, flags);

        public Task<double> HashDecrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
            => _redis.HashDecrementAsync(key, hashField, value, flags);

        public bool HashDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashDelete(key, hashField, flags);

        public long HashDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashDelete(key, hashFields, flags);

        public Task<bool> HashDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashDeleteAsync(key, hashField, flags);

        public Task<long> HashDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashDeleteAsync(key, hashFields, flags);

        public bool HashExists(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashExists(key, hashField, flags);

        public Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashExistsAsync(key, hashField, flags);

        public ExpireResult[] HashFieldExpire(RedisKey key, RedisValue[] hashFields, TimeSpan expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldExpire(key, hashFields, expiry, when, flags);

        public ExpireResult[] HashFieldExpire(RedisKey key, RedisValue[] hashFields, DateTime expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldExpire(key, hashFields, expiry, when, flags);

        public Task<ExpireResult[]> HashFieldExpireAsync(RedisKey key, RedisValue[] hashFields, TimeSpan expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldExpireAsync(key, hashFields, expiry, when, flags);

        public Task<ExpireResult[]> HashFieldExpireAsync(RedisKey key, RedisValue[] hashFields, DateTime expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldExpireAsync(key, hashFields, expiry, when, flags);

        public RedisValue HashFieldGetAndDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndDelete(key, hashField, flags);

        public RedisValue[] HashFieldGetAndDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndDelete(key, hashFields, flags);

        public Task<RedisValue> HashFieldGetAndDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndDeleteAsync(key, hashField, flags);

        public Task<RedisValue[]> HashFieldGetAndDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndDeleteAsync(key, hashFields, flags);

        public RedisValue HashFieldGetAndSetExpiry(RedisKey key, RedisValue hashField, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiry(key, hashField, expiry, persist, flags);

        public RedisValue HashFieldGetAndSetExpiry(RedisKey key, RedisValue hashField, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiry(key, hashField, expiry, flags);

        public RedisValue[] HashFieldGetAndSetExpiry(RedisKey key, RedisValue[] hashFields, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiry(key, hashFields, expiry, persist, flags);

        public RedisValue[] HashFieldGetAndSetExpiry(RedisKey key, RedisValue[] hashFields, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiry(key, hashFields, expiry, flags);

        public Task<RedisValue> HashFieldGetAndSetExpiryAsync(RedisKey key, RedisValue hashField, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiryAsync(key, hashField, expiry, persist, flags);

        public Task<RedisValue> HashFieldGetAndSetExpiryAsync(RedisKey key, RedisValue hashField, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiryAsync(key, hashField, expiry, flags);

        public Task<RedisValue[]> HashFieldGetAndSetExpiryAsync(RedisKey key, RedisValue[] hashFields, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiryAsync(key, hashFields, expiry, persist, flags);

        public Task<RedisValue[]> HashFieldGetAndSetExpiryAsync(RedisKey key, RedisValue[] hashFields, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetAndSetExpiryAsync(key, hashFields, expiry, flags);

        public long[] HashFieldGetExpireDateTime(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetExpireDateTime(key, hashFields, flags);

        public Task<long[]> HashFieldGetExpireDateTimeAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetExpireDateTimeAsync(key, hashFields, flags);

        public Lease<byte>? HashFieldGetLeaseAndDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndDelete(key, hashField, flags);

        public Task<Lease<byte>?> HashFieldGetLeaseAndDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndDeleteAsync(key, hashField, flags);

        public Lease<byte>? HashFieldGetLeaseAndSetExpiry(RedisKey key, RedisValue hashField, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndSetExpiry(key, hashField, expiry, persist, flags);

        public Lease<byte>? HashFieldGetLeaseAndSetExpiry(RedisKey key, RedisValue hashField, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndSetExpiry(key, hashField, expiry, flags);

        public Task<Lease<byte>?> HashFieldGetLeaseAndSetExpiryAsync(RedisKey key, RedisValue hashField, TimeSpan? expiry = null, bool persist = false, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndSetExpiryAsync(key, hashField, expiry, persist, flags);

        public Task<Lease<byte>?> HashFieldGetLeaseAndSetExpiryAsync(RedisKey key, RedisValue hashField, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetLeaseAndSetExpiryAsync(key, hashField, expiry, flags);

        public long[] HashFieldGetTimeToLive(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetTimeToLive(key, hashFields, flags);

        public Task<long[]> HashFieldGetTimeToLiveAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldGetTimeToLiveAsync(key, hashFields, flags);

        public PersistResult[] HashFieldPersist(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldPersist(key, hashFields, flags);

        public Task<PersistResult[]> HashFieldPersistAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldPersistAsync(key, hashFields, flags);

        public RedisValue HashFieldSetAndSetExpiry(RedisKey key, RedisValue field, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiry(key, field, value, expiry, keepTtl, when, flags);

        public RedisValue HashFieldSetAndSetExpiry(RedisKey key, RedisValue field, RedisValue value, DateTime expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiry(key, field, value, expiry, when, flags);

        public RedisValue HashFieldSetAndSetExpiry(RedisKey key, HashEntry[] hashFields, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiry(key, hashFields, expiry, keepTtl, when, flags);

        public RedisValue HashFieldSetAndSetExpiry(RedisKey key, HashEntry[] hashFields, DateTime expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiry(key, hashFields, expiry, when, flags);

        public Task<RedisValue> HashFieldSetAndSetExpiryAsync(RedisKey key, RedisValue field, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiryAsync(key, field, value, expiry, keepTtl, when, flags);

        public Task<RedisValue> HashFieldSetAndSetExpiryAsync(RedisKey key, RedisValue field, RedisValue value, DateTime expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiryAsync(key, field, value, expiry, when, flags);

        public Task<RedisValue> HashFieldSetAndSetExpiryAsync(RedisKey key, HashEntry[] hashFields, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiryAsync(key, hashFields, expiry, keepTtl, when, flags);

        public Task<RedisValue> HashFieldSetAndSetExpiryAsync(RedisKey key, HashEntry[] hashFields, DateTime expiry, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashFieldSetAndSetExpiryAsync(key, hashFields, expiry, when, flags);

        public RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashGet(key, hashField, flags);

        public RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashGet(key, hashFields, flags);

        public HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetAll(key, flags);

        public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetAllAsync(key, flags);

        public Task<RedisValue> HashGetAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetAsync(key, hashField, flags);

        public Task<RedisValue[]> HashGetAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetAsync(key, hashFields, flags);

        public Lease<byte>? HashGetLease(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetLease(key, hashField, flags);

        public Task<Lease<byte>?> HashGetLeaseAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashGetLeaseAsync(key, hashField, flags);

        public long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.HashIncrement(key, hashField, value, flags);

        public double HashIncrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
            => _redis.HashIncrement(key, hashField, value, flags);

        public Task<long> HashIncrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.HashIncrementAsync(key, hashField, value, flags);

        public Task<double> HashIncrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
            => _redis.HashIncrementAsync(key, hashField, value, flags);

        public RedisValue[] HashKeys(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashKeys(key, flags);

        public Task<RedisValue[]> HashKeysAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashKeysAsync(key, flags);

        public long HashLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashLength(key, flags);

        public Task<long> HashLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashLengthAsync(key, flags);

        public RedisValue HashRandomField(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomField(key, flags);

        public Task<RedisValue> HashRandomFieldAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomFieldAsync(key, flags);

        public RedisValue[] HashRandomFields(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomFields(key, count, flags);

        public Task<RedisValue[]> HashRandomFieldsAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomFieldsAsync(key, count, flags);

        public HashEntry[] HashRandomFieldsWithValues(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomFieldsWithValues(key, count, flags);

        public Task<HashEntry[]> HashRandomFieldsWithValuesAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.HashRandomFieldsWithValuesAsync(key, count, flags);

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
            => _redis.HashScan(key, pattern, pageSize, flags);

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.HashScan(key, pattern, pageSize, cursor, pageOffset, flags);

        public IAsyncEnumerable<HashEntry> HashScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.HashScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);

        public IEnumerable<RedisValue> HashScanNoValues(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.HashScanNoValues(key, pattern, pageSize, cursor, pageOffset, flags);

        public IAsyncEnumerable<RedisValue> HashScanNoValuesAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.HashScanNoValuesAsync(key, pattern, pageSize, cursor, pageOffset, flags);

        public void HashSet(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashSet(key, hashFields, flags);

        public bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashSet(key, hashField, value, when, flags);

        public Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
            => _redis.HashSetAsync(key, hashFields, flags);

        public Task<bool> HashSetAsync(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.HashSetAsync(key, hashField, value, when, flags);

        public long HashStringLength(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashStringLength(key, hashField, flags);

        public Task<long> HashStringLengthAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
            => _redis.HashStringLengthAsync(key, hashField, flags);

        public RedisValue[] HashValues(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashValues(key, flags);

        public Task<RedisValue[]> HashValuesAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HashValuesAsync(key, flags);

        public bool HyperLogLogAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogAdd(key, value, flags);

        public bool HyperLogLogAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogAdd(key, values, flags);

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogAddAsync(key, value, flags);

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogAddAsync(key, values, flags);

        public long HyperLogLogLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogLength(key, flags);

        public long HyperLogLogLength(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogLength(keys, flags);

        public Task<long> HyperLogLogLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogLengthAsync(key, flags);

        public Task<long> HyperLogLogLengthAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogLengthAsync(keys, flags);

        public void HyperLogLogMerge(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogMerge(destination, first, second, flags);

        public void HyperLogLogMerge(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogMerge(destination, sourceKeys, flags);

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogMergeAsync(destination, first, second, flags);

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
            => _redis.HyperLogLogMergeAsync(destination, sourceKeys, flags);

        public EndPoint? IdentifyEndpoint(RedisKey key = default, CommandFlags flags = CommandFlags.None)
            => _redis.IdentifyEndpoint(key, flags);

        public Task<EndPoint?> IdentifyEndpointAsync(RedisKey key = default, CommandFlags flags = CommandFlags.None)
            => _redis.IdentifyEndpointAsync(key, flags);

        public bool IsConnected(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.IsConnected(key, flags);

        public bool KeyCopy(RedisKey sourceKey, RedisKey destinationKey, int destinationDatabase = -1, bool replace = false, CommandFlags flags = CommandFlags.None)
            => _redis.KeyCopy(sourceKey, destinationKey, destinationDatabase, replace, flags);

        public Task<bool> KeyCopyAsync(RedisKey sourceKey, RedisKey destinationKey, int destinationDatabase = -1, bool replace = false, CommandFlags flags = CommandFlags.None)
            => _redis.KeyCopyAsync(sourceKey, destinationKey, destinationDatabase, replace, flags);

        public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDelete(key, flags);

        public long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDelete(keys, flags);

        public Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDeleteAsync(key, flags);

        public Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDeleteAsync(keys, flags);

        public byte[]? KeyDump(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDump(key, flags);

        public Task<byte[]?> KeyDumpAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyDumpAsync(key, flags);

        public string? KeyEncoding(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyEncoding(key, flags);

        public Task<string?> KeyEncodingAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyEncodingAsync(key, flags);

        public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExists(key, flags);

        public long KeyExists(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExists(keys, flags);

        public Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExistsAsync(key, flags);

        public Task<long> KeyExistsAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExistsAsync(keys, flags);

        public bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags)
            => _redis.KeyExpire(key, expiry, flags);

        public bool KeyExpire(RedisKey key, TimeSpan? expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpire(key, expiry, when, flags);

        public bool KeyExpire(RedisKey key, DateTime? expiry, CommandFlags flags)
            => _redis.KeyExpire(key, expiry, flags);

        public bool KeyExpire(RedisKey key, DateTime? expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpire(key, expiry, when, flags);

        public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags)
            => _redis.KeyExpireAsync(key, expiry, flags);

        public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpireAsync(key, expiry, when, flags);

        public Task<bool> KeyExpireAsync(RedisKey key, DateTime? expiry, CommandFlags flags)
            => _redis.KeyExpireAsync(key, expiry, flags);

        public Task<bool> KeyExpireAsync(RedisKey key, DateTime? expiry, ExpireWhen when = ExpireWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpireAsync(key, expiry, when, flags);

        public DateTime? KeyExpireTime(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpireTime(key, flags);

        public Task<DateTime?> KeyExpireTimeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyExpireTimeAsync(key, flags);

        public long? KeyFrequency(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyFrequency(key, flags);

        public Task<long?> KeyFrequencyAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyFrequencyAsync(key, flags);

        public TimeSpan? KeyIdleTime(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyIdleTime(key, flags);

        public Task<TimeSpan?> KeyIdleTimeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyIdleTimeAsync(key, flags);

        public void KeyMigrate(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
            => _redis.KeyMigrate(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);

        public Task KeyMigrateAsync(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
            => _redis.KeyMigrateAsync(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);

        public bool KeyMove(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
            => _redis.KeyMove(key, database, flags);

        public Task<bool> KeyMoveAsync(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
            => _redis.KeyMoveAsync(key, database, flags);

        public bool KeyPersist(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyPersist(key, flags);

        public Task<bool> KeyPersistAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyPersistAsync(key, flags);

        public RedisKey KeyRandom(CommandFlags flags = CommandFlags.None)
            => _redis.KeyRandom(flags);

        public Task<RedisKey> KeyRandomAsync(CommandFlags flags = CommandFlags.None)
            => _redis.KeyRandomAsync(flags);

        public long? KeyRefCount(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRefCount(key, flags);

        public Task<long?> KeyRefCountAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRefCountAsync(key, flags);

        public bool KeyRename(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRename(key, newKey, when, flags);

        public Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRenameAsync(key, newKey, when, flags);

        public void KeyRestore(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRestore(key, value, expiry, flags);

        public Task KeyRestoreAsync(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
            => _redis.KeyRestoreAsync(key, value, expiry, flags);

        public TimeSpan? KeyTimeToLive(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTimeToLive(key, flags);

        public Task<TimeSpan?> KeyTimeToLiveAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTimeToLiveAsync(key, flags);

        public bool KeyTouch(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTouch(key, flags);

        public long KeyTouch(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTouch(keys, flags);

        public Task<bool> KeyTouchAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTouchAsync(key, flags);

        public Task<long> KeyTouchAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTouchAsync(keys, flags);

        public RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyType(key, flags);

        public Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.KeyTypeAsync(key, flags);

        public RedisValue ListGetByIndex(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
            => _redis.ListGetByIndex(key, index, flags);

        public Task<RedisValue> ListGetByIndexAsync(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
            => _redis.ListGetByIndexAsync(key, index, flags);

        public long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListInsertAfter(key, pivot, value, flags);

        public Task<long> ListInsertAfterAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListInsertAfterAsync(key, pivot, value, flags);

        public long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListInsertBefore(key, pivot, value, flags);

        public Task<long> ListInsertBeforeAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListInsertBeforeAsync(key, pivot, value, flags);

        public RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPop(key, flags);

        public RedisValue[] ListLeftPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPop(key, count, flags);

        public ListPopResult ListLeftPop(RedisKey[] keys, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPop(keys, count, flags);

        public Task<RedisValue> ListLeftPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPopAsync(key, flags);

        public Task<RedisValue[]> ListLeftPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPopAsync(key, count, flags);

        public Task<ListPopResult> ListLeftPopAsync(RedisKey[] keys, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPopAsync(keys, count, flags);

        public long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPush(key, value, when, flags);

        public long ListLeftPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPush(key, values, when, flags);

        public long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags)
            => _redis.ListLeftPush(key, values, flags);

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPushAsync(key, value, when, flags);

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListLeftPushAsync(key, values, when, flags);

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags)
            => _redis.ListLeftPushAsync(key, values, flags);

        public long ListLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListLength(key, flags);

        public Task<long> ListLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListLengthAsync(key, flags);

        public RedisValue ListMove(RedisKey sourceKey, RedisKey destinationKey, ListSide sourceSide, ListSide destinationSide, CommandFlags flags = CommandFlags.None)
            => _redis.ListMove(sourceKey, destinationKey, sourceSide, destinationSide, flags);

        public Task<RedisValue> ListMoveAsync(RedisKey sourceKey, RedisKey destinationKey, ListSide sourceSide, ListSide destinationSide, CommandFlags flags = CommandFlags.None)
            => _redis.ListMoveAsync(sourceKey, destinationKey, sourceSide, destinationSide, flags);

        public long ListPosition(RedisKey key, RedisValue element, long rank = 1, long maxLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListPosition(key, element, rank, maxLength, flags);

        public Task<long> ListPositionAsync(RedisKey key, RedisValue element, long rank = 1, long maxLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListPositionAsync(key, element, rank, maxLength, flags);

        public long[] ListPositions(RedisKey key, RedisValue element, long count, long rank = 1, long maxLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListPositions(key, element, count, rank, maxLength, flags);

        public Task<long[]> ListPositionsAsync(RedisKey key, RedisValue element, long count, long rank = 1, long maxLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListPositionsAsync(key, element, count, rank, maxLength, flags);

        public RedisValue[] ListRange(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
            => _redis.ListRange(key, start, stop, flags);

        public Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
            => _redis.ListRangeAsync(key, start, stop, flags);

        public long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListRemove(key, value, count, flags);

        public Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
            => _redis.ListRemoveAsync(key, value, count, flags);

        public RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPop(key, flags);

        public RedisValue[] ListRightPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPop(key, count, flags);

        public ListPopResult ListRightPop(RedisKey[] keys, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPop(keys, count, flags);

        public Task<RedisValue> ListRightPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPopAsync(key, flags);

        public Task<RedisValue[]> ListRightPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPopAsync(key, count, flags);

        public Task<ListPopResult> ListRightPopAsync(RedisKey[] keys, long count, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPopAsync(keys, count, flags);

        public RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPopLeftPush(source, destination, flags);

        public Task<RedisValue> ListRightPopLeftPushAsync(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPopLeftPushAsync(source, destination, flags);

        public long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPush(key, value, when, flags);

        public long ListRightPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPush(key, values, when, flags);

        public long ListRightPush(RedisKey key, RedisValue[] values, CommandFlags flags)
            => _redis.ListRightPush(key, values, flags);

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPushAsync(key, value, when, flags);

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.ListRightPushAsync(key, values, when, flags);

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags)
            => _redis.ListRightPushAsync(key, values, flags);

        public void ListSetByIndex(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListSetByIndex(key, index, value, flags);

        public Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.ListSetByIndexAsync(key, index, value, flags);

        public void ListTrim(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
            => _redis.ListTrim(key, start, stop, flags);

        public Task ListTrimAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
            => _redis.ListTrimAsync(key, start, stop, flags);

        public bool LockExtend(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
            => _redis.LockExtend(key, value, expiry, flags);

        public Task<bool> LockExtendAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
            => _redis.LockExtendAsync(key, value, expiry, flags);

        public RedisValue LockQuery(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.LockQuery(key, flags);

        public Task<RedisValue> LockQueryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.LockQueryAsync(key, flags);

        public bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.LockRelease(key, value, flags);

        public Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.LockReleaseAsync(key, value, flags);

        public bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
            => _redis.LockTake(key, value, expiry, flags);

        public Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
            => _redis.LockTakeAsync(key, value, expiry, flags);

        public TimeSpan Ping(CommandFlags flags = CommandFlags.None)
            => _redis.Ping(flags);

        public Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
            => _redis.PingAsync(flags);

        public long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
            => _redis.Publish(channel, message, flags);

        public Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
            => _redis.PublishAsync(channel, message, flags);

        public RedisResult ScriptEvaluate(string script, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluate(script, keys, values, flags);

        public RedisResult ScriptEvaluate(byte[] hash, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluate(hash, keys, values, flags);

        public RedisResult ScriptEvaluate(LuaScript script, object? parameters = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluate(script, parameters, flags);

        public RedisResult ScriptEvaluate(LoadedLuaScript script, object? parameters = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluate(script, parameters, flags);

        public Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateAsync(script, keys, values, flags);

        public Task<RedisResult> ScriptEvaluateAsync(byte[] hash, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateAsync(hash, keys, values, flags);

        public Task<RedisResult> ScriptEvaluateAsync(LuaScript script, object? parameters = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateAsync(script, parameters, flags);

        public Task<RedisResult> ScriptEvaluateAsync(LoadedLuaScript script, object? parameters = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateAsync(script, parameters, flags);

        public RedisResult ScriptEvaluateReadOnly(string script, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateReadOnly(script, keys, values, flags);

        public RedisResult ScriptEvaluateReadOnly(byte[] hash, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateReadOnly(hash, keys, values, flags);

        public Task<RedisResult> ScriptEvaluateReadOnlyAsync(string script, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateReadOnlyAsync(script, keys, values, flags);

        public Task<RedisResult> ScriptEvaluateReadOnlyAsync(byte[] hash, RedisKey[]? keys = null, RedisValue[]? values = null, CommandFlags flags = CommandFlags.None)
            => _redis.ScriptEvaluateReadOnlyAsync(hash, keys, values, flags);

        public bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetAdd(key, value, flags);

        public long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetAdd(key, values, flags);

        public Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetAddAsync(key, value, flags);

        public Task<long> SetAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetAddAsync(key, values, flags);

        public RedisValue[] SetCombine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombine(operation, first, second, flags);

        public RedisValue[] SetCombine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombine(operation, keys, flags);

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAndStore(operation, destination, first, second, flags);

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAndStore(operation, destination, keys, flags);

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAndStoreAsync(operation, destination, first, second, flags);

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAndStoreAsync(operation, destination, keys, flags);

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAsync(operation, first, second, flags);

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.SetCombineAsync(operation, keys, flags);

        public bool SetContains(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetContains(key, value, flags);

        public bool[] SetContains(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetContains(key, values, flags);

        public Task<bool> SetContainsAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetContainsAsync(key, value, flags);

        public Task<bool[]> SetContainsAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetContainsAsync(key, values, flags);

        public long SetIntersectionLength(RedisKey[] keys, long limit = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SetIntersectionLength(keys, limit, flags);

        public Task<long> SetIntersectionLengthAsync(RedisKey[] keys, long limit = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SetIntersectionLengthAsync(keys, limit, flags);

        public long SetLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetLength(key, flags);

        public Task<long> SetLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetLengthAsync(key, flags);

        public RedisValue[] SetMembers(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetMembers(key, flags);

        public Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetMembersAsync(key, flags);

        public bool SetMove(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetMove(source, destination, value, flags);

        public Task<bool> SetMoveAsync(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetMoveAsync(source, destination, value, flags);

        public RedisValue SetPop(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetPop(key, flags);

        public RedisValue[] SetPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SetPop(key, count, flags);

        public Task<RedisValue> SetPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetPopAsync(key, flags);

        public Task<RedisValue[]> SetPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SetPopAsync(key, count, flags);

        public RedisValue SetRandomMember(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetRandomMember(key, flags);

        public Task<RedisValue> SetRandomMemberAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SetRandomMemberAsync(key, flags);

        public RedisValue[] SetRandomMembers(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SetRandomMembers(key, count, flags);

        public Task<RedisValue[]> SetRandomMembersAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SetRandomMembersAsync(key, count, flags);

        public bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetRemove(key, value, flags);

        public long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetRemove(key, values, flags);

        public Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.SetRemoveAsync(key, value, flags);

        public Task<long> SetRemoveAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
            => _redis.SetRemoveAsync(key, values, flags);

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
            => _redis.SetScan(key, pattern, pageSize, flags);

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SetScan(key, pattern, pageSize, cursor, pageOffset, flags);

        public IAsyncEnumerable<RedisValue> SetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SetScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);

        public RedisValue[] Sort(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[]? get = null, CommandFlags flags = CommandFlags.None)
            => _redis.Sort(key, skip, take, order, sortType, by, get, flags);

        public long SortAndStore(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[]? get = null, CommandFlags flags = CommandFlags.None)
            => _redis.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);

        public Task<long> SortAndStoreAsync(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[]? get = null, CommandFlags flags = CommandFlags.None)
            => _redis.SortAndStoreAsync(destination, key, skip, take, order, sortType, by, get, flags);

        public Task<RedisValue[]> SortAsync(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[]? get = null, CommandFlags flags = CommandFlags.None)
            => _redis.SortAsync(key, skip, take, order, sortType, by, get, flags);

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, CommandFlags flags)
            => _redis.SortedSetAdd(key, member, score, flags);

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, When when, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAdd(key, member, score, when, flags);

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAdd(key, member, score, when, flags);

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
            => _redis.SortedSetAdd(key, values, flags);

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, When when, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAdd(key, values, when, flags);

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAdd(key, values, when, flags);

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, CommandFlags flags)
            => _redis.SortedSetAddAsync(key, member, score, flags);

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAddAsync(key, member, score, when, flags);

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAddAsync(key, member, score, when, flags);

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
            => _redis.SortedSetAddAsync(key, values, flags);

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, When when, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAddAsync(key, values, when, flags);

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetAddAsync(key, values, when, flags);

        public RedisValue[] SortedSetCombine(SetOperation operation, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombine(operation, keys, weights, aggregate, flags);

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineAndStoreAsync(operation, destination, first, second, aggregate, flags);

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineAndStoreAsync(operation, destination, keys, weights, aggregate, flags);

        public Task<RedisValue[]> SortedSetCombineAsync(SetOperation operation, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineAsync(operation, keys, weights, aggregate, flags);

        public SortedSetEntry[] SortedSetCombineWithScores(SetOperation operation, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineWithScores(operation, keys, weights, aggregate, flags);

        public Task<SortedSetEntry[]> SortedSetCombineWithScoresAsync(SetOperation operation, RedisKey[] keys, double[]? weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetCombineWithScoresAsync(operation, keys, weights, aggregate, flags);

        public double SortedSetDecrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetDecrement(key, member, value, flags);

        public Task<double> SortedSetDecrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetDecrementAsync(key, member, value, flags);

        public double SortedSetIncrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetIncrement(key, member, value, flags);

        public Task<double> SortedSetIncrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetIncrementAsync(key, member, value, flags);

        public long SortedSetIntersectionLength(RedisKey[] keys, long limit = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetIntersectionLength(keys, limit, flags);

        public Task<long> SortedSetIntersectionLengthAsync(RedisKey[] keys, long limit = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetIntersectionLengthAsync(keys, limit, flags);

        public long SortedSetLength(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetLength(key, min, max, exclude, flags);

        public Task<long> SortedSetLengthAsync(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetLengthAsync(key, min, max, exclude, flags);

        public long SortedSetLengthByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetLengthByValue(key, min, max, exclude, flags);

        public Task<long> SortedSetLengthByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetLengthByValueAsync(key, min, max, exclude, flags);

        public SortedSetEntry? SortedSetPop(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPop(key, order, flags);

        public SortedSetEntry[] SortedSetPop(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPop(key, count, order, flags);

        public SortedSetPopResult SortedSetPop(RedisKey[] keys, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPop(keys, count, order, flags);

        public Task<SortedSetEntry?> SortedSetPopAsync(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPopAsync(key, order, flags);

        public Task<SortedSetEntry[]> SortedSetPopAsync(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPopAsync(key, count, order, flags);

        public Task<SortedSetPopResult> SortedSetPopAsync(RedisKey[] keys, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetPopAsync(keys, count, order, flags);

        public RedisValue SortedSetRandomMember(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMember(key, flags);

        public Task<RedisValue> SortedSetRandomMemberAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMemberAsync(key, flags);

        public RedisValue[] SortedSetRandomMembers(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMembers(key, count, flags);

        public Task<RedisValue[]> SortedSetRandomMembersAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMembersAsync(key, count, flags);

        public SortedSetEntry[] SortedSetRandomMembersWithScores(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMembersWithScores(key, count, flags);

        public Task<SortedSetEntry[]> SortedSetRandomMembersWithScoresAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRandomMembersWithScoresAsync(key, count, flags);

        public long SortedSetRangeAndStore(RedisKey sourceKey, RedisKey destinationKey, RedisValue start, RedisValue stop, SortedSetOrder sortedSetOrder = SortedSetOrder.ByRank, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long? take = null, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeAndStore(sourceKey, destinationKey, start, stop, sortedSetOrder, exclude, order, skip, take, flags);

        public Task<long> SortedSetRangeAndStoreAsync(RedisKey sourceKey, RedisKey destinationKey, RedisValue start, RedisValue stop, SortedSetOrder sortedSetOrder = SortedSetOrder.ByRank, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long? take = null, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeAndStoreAsync(sourceKey, destinationKey, start, stop, sortedSetOrder, exclude, order, skip, take, flags);

        public RedisValue[] SortedSetRangeByRank(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByRank(key, start, stop, order, flags);

        public Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByRankAsync(key, start, stop, order, flags);

        public SortedSetEntry[] SortedSetRangeByRankWithScores(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByRankWithScores(key, start, stop, order, flags);

        public Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByRankWithScoresAsync(key, start, stop, order, flags);

        public RedisValue[] SortedSetRangeByScore(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByScore(key, start, stop, exclude, order, skip, take, flags);

        public Task<RedisValue[]> SortedSetRangeByScoreAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByScoreAsync(key, start, stop, exclude, order, skip, take, flags);

        public SortedSetEntry[] SortedSetRangeByScoreWithScores(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByScoreWithScores(key, start, stop, exclude, order, skip, take, flags);

        public Task<SortedSetEntry[]> SortedSetRangeByScoreWithScoresAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByScoreWithScoresAsync(key, start, stop, exclude, order, skip, take, flags);

        public RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByValue(key, min, max, exclude, skip, take, flags);

        public RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByValue(key, min, max, exclude, order, skip, take, flags);

        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByValueAsync(key, min, max, exclude, skip, take, flags);

        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRangeByValueAsync(key, min, max, exclude, order, skip, take, flags);

        public long? SortedSetRank(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRank(key, member, order, flags);

        public Task<long?> SortedSetRankAsync(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRankAsync(key, member, order, flags);

        public bool SortedSetRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemove(key, member, flags);

        public long SortedSetRemove(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemove(key, members, flags);

        public Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveAsync(key, member, flags);

        public Task<long> SortedSetRemoveAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveAsync(key, members, flags);

        public long SortedSetRemoveRangeByRank(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByRank(key, start, stop, flags);

        public Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByRankAsync(key, start, stop, flags);

        public long SortedSetRemoveRangeByScore(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);

        public Task<long> SortedSetRemoveRangeByScoreAsync(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude, flags);

        public long SortedSetRemoveRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByValue(key, min, max, exclude, flags);

        public Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
            => _redis.SortedSetScan(key, pattern, pageSize, flags);

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScan(key, pattern, pageSize, cursor, pageOffset, flags);

        public IAsyncEnumerable<SortedSetEntry> SortedSetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);

        public double? SortedSetScore(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScore(key, member, flags);

        public Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScoreAsync(key, member, flags);

        public double?[] SortedSetScores(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScores(key, members, flags);

        public Task<double?[]> SortedSetScoresAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetScoresAsync(key, members, flags);

        public bool SortedSetUpdate(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetUpdate(key, member, score, when, flags);

        public long SortedSetUpdate(RedisKey key, SortedSetEntry[] values, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetUpdate(key, values, when, flags);

        public Task<bool> SortedSetUpdateAsync(RedisKey key, RedisValue member, double score, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetUpdateAsync(key, member, score, when, flags);

        public Task<long> SortedSetUpdateAsync(RedisKey key, SortedSetEntry[] values, SortedSetWhen when = SortedSetWhen.Always, CommandFlags flags = CommandFlags.None)
            => _redis.SortedSetUpdateAsync(key, values, when, flags);

        public long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledge(key, groupName, messageId, flags);

        public long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledge(key, groupName, messageIds, flags);

        public StreamTrimResult StreamAcknowledgeAndDelete(RedisKey key, RedisValue groupName, StreamTrimMode mode, RedisValue messageId, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAndDelete(key, groupName, mode, messageId, flags);

        public StreamTrimResult[] StreamAcknowledgeAndDelete(RedisKey key, RedisValue groupName, StreamTrimMode mode, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAndDelete(key, groupName, mode, messageIds, flags);

        public Task<StreamTrimResult> StreamAcknowledgeAndDeleteAsync(RedisKey key, RedisValue groupName, StreamTrimMode mode, RedisValue messageId, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAndDeleteAsync(key, groupName, mode, messageId, flags);

        public Task<StreamTrimResult[]> StreamAcknowledgeAndDeleteAsync(RedisKey key, RedisValue groupName, StreamTrimMode mode, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAndDeleteAsync(key, groupName, mode, messageIds, flags);

        public Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAsync(key, groupName, messageId, flags);

        public Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAcknowledgeAsync(key, groupName, messageIds, flags);

        public RedisValue StreamAdd(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId, int? maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamAdd(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, flags);

        public RedisValue StreamAdd(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId, int? maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamAdd(key, streamPairs, messageId, maxLength, useApproximateMaxLength, flags);

        public RedisValue StreamAdd(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, long? maxLength = null, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode trimMode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAdd(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, limit, trimMode, flags);

        public RedisValue StreamAdd(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, long? maxLength = null, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode trimMode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAdd(key, streamPairs, messageId, maxLength, useApproximateMaxLength, limit, trimMode, flags);

        public Task<RedisValue> StreamAddAsync(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId, int? maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamAddAsync(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, flags);

        public Task<RedisValue> StreamAddAsync(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId, int? maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamAddAsync(key, streamPairs, messageId, maxLength, useApproximateMaxLength, flags);

        public Task<RedisValue> StreamAddAsync(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, long? maxLength = null, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode trimMode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAddAsync(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, limit, trimMode, flags);

        public Task<RedisValue> StreamAddAsync(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, long? maxLength = null, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode trimMode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAddAsync(key, streamPairs, messageId, maxLength, useApproximateMaxLength, limit, trimMode, flags);

        public StreamAutoClaimResult StreamAutoClaim(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue startAtId, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAutoClaim(key, consumerGroup, claimingConsumer, minIdleTimeInMs, startAtId, count, flags);

        public Task<StreamAutoClaimResult> StreamAutoClaimAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue startAtId, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAutoClaimAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, startAtId, count, flags);

        public StreamAutoClaimIdsOnlyResult StreamAutoClaimIdsOnly(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue startAtId, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAutoClaimIdsOnly(key, consumerGroup, claimingConsumer, minIdleTimeInMs, startAtId, count, flags);

        public Task<StreamAutoClaimIdsOnlyResult> StreamAutoClaimIdsOnlyAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue startAtId, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamAutoClaimIdsOnlyAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, startAtId, count, flags);

        public StreamEntry[] StreamClaim(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamClaim(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);

        public Task<StreamEntry[]> StreamClaimAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamClaimAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);

        public RedisValue[] StreamClaimIdsOnly(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamClaimIdsOnly(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);

        public Task<RedisValue[]> StreamClaimIdsOnlyAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamClaimIdsOnlyAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);

        public bool StreamConsumerGroupSetPosition(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
            => _redis.StreamConsumerGroupSetPosition(key, groupName, position, flags);

        public Task<bool> StreamConsumerGroupSetPositionAsync(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
            => _redis.StreamConsumerGroupSetPositionAsync(key, groupName, position, flags);

        public StreamConsumerInfo[] StreamConsumerInfo(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamConsumerInfo(key, groupName, flags);

        public Task<StreamConsumerInfo[]> StreamConsumerInfoAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamConsumerInfoAsync(key, groupName, flags);

        public bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
            => _redis.StreamCreateConsumerGroup(key, groupName, position, flags);

        public bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
            => _redis.StreamCreateConsumerGroup(key, groupName, position, createStream, flags);

        public Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
            => _redis.StreamCreateConsumerGroupAsync(key, groupName, position, flags);

        public Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
            => _redis.StreamCreateConsumerGroupAsync(key, groupName, position, createStream, flags);

        public long StreamDelete(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDelete(key, messageIds, flags);

        public StreamTrimResult[] StreamDelete(RedisKey key, RedisValue[] messageIds, StreamTrimMode mode, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDelete(key, messageIds, mode, flags);

        public Task<long> StreamDeleteAsync(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteAsync(key, messageIds, flags);

        public Task<StreamTrimResult[]> StreamDeleteAsync(RedisKey key, RedisValue[] messageIds, StreamTrimMode mode, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteAsync(key, messageIds, mode, flags);

        public long StreamDeleteConsumer(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteConsumer(key, groupName, consumerName, flags);

        public Task<long> StreamDeleteConsumerAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteConsumerAsync(key, groupName, consumerName, flags);

        public bool StreamDeleteConsumerGroup(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteConsumerGroup(key, groupName, flags);

        public Task<bool> StreamDeleteConsumerGroupAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamDeleteConsumerGroupAsync(key, groupName, flags);

        public StreamGroupInfo[] StreamGroupInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamGroupInfo(key, flags);

        public Task<StreamGroupInfo[]> StreamGroupInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamGroupInfoAsync(key, flags);

        public StreamInfo StreamInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamInfo(key, flags);

        public Task<StreamInfo> StreamInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamInfoAsync(key, flags);

        public long StreamLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamLength(key, flags);

        public Task<long> StreamLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StreamLengthAsync(key, flags);

        public StreamPendingInfo StreamPending(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamPending(key, groupName, flags);

        public Task<StreamPendingInfo> StreamPendingAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
            => _redis.StreamPendingAsync(key, groupName, flags);

        public StreamPendingMessageInfo[] StreamPendingMessages(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId, RedisValue? maxId, CommandFlags flags)
            => _redis.StreamPendingMessages(key, groupName, count, consumerName, minId, maxId, flags);

        public StreamPendingMessageInfo[] StreamPendingMessages(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, long? minIdleTimeInMs = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamPendingMessages(key, groupName, count, consumerName, minId, maxId, minIdleTimeInMs, flags);

        public Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId, RedisValue? maxId, CommandFlags flags)
            => _redis.StreamPendingMessagesAsync(key, groupName, count, consumerName, minId, maxId, flags);

        public Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, long? minIdleTimeInMs = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamPendingMessagesAsync(key, groupName, count, consumerName, minId, maxId, minIdleTimeInMs, flags);

        public StreamEntry[] StreamRange(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.StreamRange(key, minId, maxId, count, messageOrder, flags);

        public Task<StreamEntry[]> StreamRangeAsync(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
            => _redis.StreamRangeAsync(key, minId, maxId, count, messageOrder, flags);

        public StreamEntry[] StreamRead(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamRead(key, position, count, flags);

        public RedisStream[] StreamRead(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamRead(streamPositions, countPerStream, flags);

        public Task<StreamEntry[]> StreamReadAsync(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadAsync(key, position, count, flags);

        public Task<RedisStream[]> StreamReadAsync(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadAsync(streamPositions, countPerStream, flags);

        public StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadGroup(key, groupName, consumerName, position, count, flags);

        public StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadGroup(key, groupName, consumerName, position, count, noAck, flags);

        public RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
            => _redis.StreamReadGroup(streamPositions, groupName, consumerName, countPerStream, flags);

        public RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadGroup(streamPositions, groupName, consumerName, countPerStream, noAck, flags);

        public Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags)
            => _redis.StreamReadGroupAsync(key, groupName, consumerName, position, count, flags);

        public Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadGroupAsync(key, groupName, consumerName, position, count, noAck, flags);

        public Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
            => _redis.StreamReadGroupAsync(streamPositions, groupName, consumerName, countPerStream, flags);

        public Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
            => _redis.StreamReadGroupAsync(streamPositions, groupName, consumerName, countPerStream, noAck, flags);

        public long StreamTrim(RedisKey key, int maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamTrim(key, maxLength, useApproximateMaxLength, flags);

        public long StreamTrim(RedisKey key, long maxLength, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode mode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamTrim(key, maxLength, useApproximateMaxLength, limit, mode, flags);

        public Task<long> StreamTrimAsync(RedisKey key, int maxLength, bool useApproximateMaxLength, CommandFlags flags)
            => _redis.StreamTrimAsync(key, maxLength, useApproximateMaxLength, flags);

        public Task<long> StreamTrimAsync(RedisKey key, long maxLength, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode mode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamTrimAsync(key, maxLength, useApproximateMaxLength, limit, mode, flags);

        public long StreamTrimByMinId(RedisKey key, RedisValue minId, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode mode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamTrimByMinId(key, minId, useApproximateMaxLength, limit, mode, flags);

        public Task<long> StreamTrimByMinIdAsync(RedisKey key, RedisValue minId, bool useApproximateMaxLength = false, long? limit = null, StreamTrimMode mode = StreamTrimMode.KeepReferences, CommandFlags flags = CommandFlags.None)
            => _redis.StreamTrimByMinIdAsync(key, minId, useApproximateMaxLength, limit, mode, flags);

        public long StringAppend(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.StringAppend(key, value, flags);

        public Task<long> StringAppendAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.StringAppendAsync(key, value, flags);

        public long StringBitCount(RedisKey key, long start, long end, CommandFlags flags)
            => _redis.StringBitCount(key, start, end, flags);

        public long StringBitCount(RedisKey key, long start = 0, long end = -1, StringIndexType indexType = StringIndexType.Byte, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitCount(key, start, end, indexType, flags);

        public Task<long> StringBitCountAsync(RedisKey key, long start, long end, CommandFlags flags)
            => _redis.StringBitCountAsync(key, start, end, flags);

        public Task<long> StringBitCountAsync(RedisKey key, long start = 0, long end = -1, StringIndexType indexType = StringIndexType.Byte, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitCountAsync(key, start, end, indexType, flags);

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitOperation(operation, destination, first, second, flags);

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitOperation(operation, destination, keys, flags);

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitOperationAsync(operation, destination, first, second, flags);

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitOperationAsync(operation, destination, keys, flags);

        public long StringBitPosition(RedisKey key, bool bit, long start, long end, CommandFlags flags)
            => _redis.StringBitPosition(key, bit, start, end, flags);

        public long StringBitPosition(RedisKey key, bool bit, long start = 0, long end = -1, StringIndexType indexType = StringIndexType.Byte, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitPosition(key, bit, start, end, indexType, flags);

        public Task<long> StringBitPositionAsync(RedisKey key, bool bit, long start, long end, CommandFlags flags)
            => _redis.StringBitPositionAsync(key, bit, start, end, flags);

        public Task<long> StringBitPositionAsync(RedisKey key, bool bit, long start = 0, long end = -1, StringIndexType indexType = StringIndexType.Byte, CommandFlags flags = CommandFlags.None)
            => _redis.StringBitPositionAsync(key, bit, start, end, indexType, flags);

        public long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.StringDecrement(key, value, flags);

        public double StringDecrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
            => _redis.StringDecrement(key, value, flags);

        public Task<long> StringDecrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.StringDecrementAsync(key, value, flags);

        public Task<double> StringDecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
            => _redis.StringDecrementAsync(key, value, flags);

        public RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGet(key, flags);

        public RedisValue[] StringGet(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.StringGet(keys, flags);

        public Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetAsync(key, flags);

        public Task<RedisValue[]> StringGetAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetAsync(keys, flags);

        public bool StringGetBit(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetBit(key, offset, flags);

        public Task<bool> StringGetBitAsync(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetBitAsync(key, offset, flags);

        public RedisValue StringGetDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetDelete(key, flags);

        public Task<RedisValue> StringGetDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetDeleteAsync(key, flags);

        public Lease<byte>? StringGetLease(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetLease(key, flags);

        public Task<Lease<byte>?> StringGetLeaseAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetLeaseAsync(key, flags);

        public RedisValue StringGetRange(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetRange(key, start, end, flags);

        public Task<RedisValue> StringGetRangeAsync(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetRangeAsync(key, start, end, flags);

        public RedisValue StringGetSet(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSet(key, value, flags);

        public Task<RedisValue> StringGetSetAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSetAsync(key, value, flags);

        public RedisValue StringGetSetExpiry(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSetExpiry(key, expiry, flags);

        public RedisValue StringGetSetExpiry(RedisKey key, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSetExpiry(key, expiry, flags);

        public Task<RedisValue> StringGetSetExpiryAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSetExpiryAsync(key, expiry, flags);

        public Task<RedisValue> StringGetSetExpiryAsync(RedisKey key, DateTime expiry, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetSetExpiryAsync(key, expiry, flags);

        public RedisValueWithExpiry StringGetWithExpiry(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetWithExpiry(key, flags);

        public Task<RedisValueWithExpiry> StringGetWithExpiryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringGetWithExpiryAsync(key, flags);

        public long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.StringIncrement(key, value, flags);

        public double StringIncrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
            => _redis.StringIncrement(key, value, flags);

        public Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
            => _redis.StringIncrementAsync(key, value, flags);

        public Task<double> StringIncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
            => _redis.StringIncrementAsync(key, value, flags);

        public long StringLength(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringLength(key, flags);

        public Task<long> StringLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
            => _redis.StringLengthAsync(key, flags);

        public string? StringLongestCommonSubsequence(RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequence(first, second, flags);

        public Task<string?> StringLongestCommonSubsequenceAsync(RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequenceAsync(first, second, flags);

        public Task<LCSMatchResult> StringLongestCommonSubsequenceWithMatchesAsync(RedisKey first, RedisKey second, long minLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequenceWithMatchesAsync(first, second, minLength, flags);

        public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry, When when)
            => _redis.StringSet(key, value, expiry, when);

        public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
            => _redis.StringSet(key, value, expiry, when, flags);

        public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSet(key, value, expiry, keepTtl, when, flags);

        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSet(values, when, flags);

        public RedisValue StringSetAndGet(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
            => _redis.StringSetAndGet(key, value, expiry, when, flags);

        public RedisValue StringSetAndGet(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSetAndGet(key, value, expiry, keepTtl, when, flags);

        public Task<RedisValue> StringSetAndGetAsync(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
            => _redis.StringSetAndGetAsync(key, value, expiry, when, flags);

        public Task<RedisValue> StringSetAndGetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSetAndGetAsync(key, value, expiry, keepTtl, when);

        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry, When when)
            => _redis.StringSetAsync(key, value, expiry, when);

        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
            => _redis.StringSetAsync(key, value, expiry, when);

        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSetAsync(key, value, expiry, keepTtl, when);

        public Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
            => _redis.StringSetAsync(values, when, flags);

        public bool StringSetBit(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None) => _redis.StringSetBit(key, offset, bit, flags);

        public Task<bool> StringSetBitAsync(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None) => _redis.StringSetBitAsync(key, offset, bit, flags);

        public RedisValue StringSetRange(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None) => _redis.StringSetRange(key, offset, value, flags);

        public Task<RedisValue> StringSetRangeAsync(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None) => _redis.StringSetRangeAsync(key, offset, value, flags);

        public bool TryWait(Task task) => _redis.TryWait(task);

        public void Wait(Task task) => _redis.Wait(task);

        public T Wait<T>(Task<T> task) => _redis.Wait(task);

        public void WaitAll(params Task[] tasks) => _redis.WaitAll(tasks);

        public long StringLongestCommonSubsequenceLength(RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequenceLength(first, second, flags);

        public LCSMatchResult StringLongestCommonSubsequenceWithMatches(RedisKey first, RedisKey second, long minLength = 0, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequenceWithMatches(first, second, minLength, flags);

        public Task<long> StringLongestCommonSubsequenceLengthAsync(RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
            => _redis.StringLongestCommonSubsequenceLengthAsync(first, second, flags);
    }
}