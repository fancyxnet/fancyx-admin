
using System.Security.Claims;

using Fancyx.Shared.Consts;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Fancyx.Shared.WebApi.Handlers
{
    internal class CommonAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
        private readonly PermissionCacheHandler _permissionCache;

        public CommonAuthorizationMiddlewareResultHandler(PermissionCacheHandler permissionCache)
        {
            _permissionCache = permissionCache;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                //身份验证
                var subjectId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var sessionId = context.User.FindFirst(x => x.Type == AdminConsts.SessionId)?.Value;

                var requestToken = context.Request.Headers.Authorization.ToString().Replace(JwtBearerDefaults.AuthenticationScheme, "").Trim();
                var isValid = await _permissionCache.CheckTokenAsync(subjectId!, sessionId!, requestToken);
                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new AppResponse<bool>(ErrorCode.NoAuth, "身份信息已过期，请重新登录").SetData(false));
                    return;
                }

                //检查权限
                var endpoint = context.GetEndpoint();
                var auth = endpoint?.Metadata.GetMetadata<HasPermissionAttribute>();
                var hasPower = true;
                if (hasPower && auth != null)
                {
                    hasPower = await _permissionCache.CheckPermissionAsync(subjectId!, auth.Code);
                }

                if (!hasPower)
                {
                    await context.Response.WriteAsJsonAsync(new AppResponse<bool>(ErrorCode.Forbidden, "权限不足，请联系管理员").SetData(false));
                    return;
                }
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
