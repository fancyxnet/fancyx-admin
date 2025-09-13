namespace Fancyx.Redis
{
    public interface IHybridCache
    {
        /// <summary>
        /// 获取或创建缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="expiration"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory,
            TimeSpan? expiration = null, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string key, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value,
            TimeSpan? expiration = null, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task RemoveAsync(string key, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 正则匹配移除缓存项
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task RemoveByPatternAsync(string pattern, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 检查缓存项是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key, HybridCacheMode mode = HybridCacheMode.Both);

        /// <summary>
        /// 获取正则匹配缓存KEY
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        Task<List<string>> KeyPatternAsync(string pattern, HybridCacheMode mode = HybridCacheMode.Both);
    }
}