using Cracker.Caching;
using Cracker.IdentityServer.Abstractions;
using Fancyx.Internal.Grpc.System;
using Fancyx.Shared.Keys;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Shared.WebApi.Handlers
{
    public class PermissionCacheHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public Auth.AuthClient AuthClient { get; }

        public PermissionCacheHandler(Auth.AuthClient authClient, IServiceProvider serviceProvider)
        {
            AuthClient = authClient;
            _serviceProvider = serviceProvider;
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
            await using var scoped = _serviceProvider.CreateAsyncScope();
            var cache = RedisHelper.Instance.CreateDatabase(prefix: $"tenant:{TenantManager.Current}:");

            string key = SystemCacheKey.AccessToken(userId, sessionId);
            var existToken = await cache.StringGetAsync(key);
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
