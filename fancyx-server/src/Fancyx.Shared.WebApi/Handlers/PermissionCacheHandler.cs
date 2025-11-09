using Fancyx.Internal.Grpc.System;
using Fancyx.Redis;
using Fancyx.Shared.Keys;

namespace Fancyx.Shared.WebApi.Handlers
{
    public class PermissionCacheHandler
    {
        private readonly IHybridCache Cache;

        public Auth.AuthClient AuthClient { get; }

        public PermissionCacheHandler(IHybridCache hybridCache, Auth.AuthClient authClient)
        {
            Cache = hybridCache;
            AuthClient = authClient;
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
            var existToken = await Cache.GetAsync<string>(key);
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
            var res = await AuthClient.GetUserPermissionAsync(new GetUserPermissionReq { UserId = long.Parse(userId) });
            return res.Auths.Contains(code);
        }
    }
}
