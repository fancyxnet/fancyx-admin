using StackExchange.Redis;

namespace Fancyx.Cache
{
    public interface ICacheClient : IDatabase
    {
        #region custom

        void WithKeyPrefix(string prefix);

        Task SetAsync<T>(string key, T value, TimeSpan? expire = null);

        Task<T?> GetAsync<T>(string key);

        Task<string[]?> KeyPatternAsync(string pattern, int count = 100);

        Task KeyDeleteByPatternAsync(string pattern);

        #endregion
    }
}
