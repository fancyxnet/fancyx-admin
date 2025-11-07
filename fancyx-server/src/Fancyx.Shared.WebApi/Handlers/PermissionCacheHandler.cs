using Fancyx.Redis;
using Fancyx.Shared.Keys;
using Fancyx.Shared.Models;

namespace Fancyx.Shared.WebApi.Handlers
{
    public class PermissionCacheHandler
    {
        private readonly IHybridCache _hybridCache;

        public PermissionCacheHandler(IHybridCache hybridCache)
        {
            _hybridCache = hybridCache;
        }

        /// <summary>
        /// 检查Token是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sessionId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CheckTokenAsync(string userId, string sessionId, string token)
        {
            string key = SystemCacheKey.AccessToken(userId, sessionId);
            var existToken = await _hybridCache.GetAsync<string>(key);
            return existToken == token;
        }

        /// <summary>
        /// 检查用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckPermissionAsync(string userId, string code)
        {
            // TODO: 如果缓存不存在，要读数据库
            var key = SystemCacheKey.UserPermission(userId);
            if (await _hybridCache.ExistsAsync(key))
            {
                var permission = await _hybridCache.GetAsync<UserPermission>(key);
                if (permission == null || permission.Auths == null) return false;

                return permission.Auths.Contains(code);
            }
            return false;
        }
    }
}
