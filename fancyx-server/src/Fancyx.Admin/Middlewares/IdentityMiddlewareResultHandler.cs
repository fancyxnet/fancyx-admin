using Fancyx.Admin.SharedService;
using Fancyx.Core.Attributes;
using Fancyx.Shared.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Security.Claims;

namespace Fancyx.Admin.Middlewares
{
    public class IdentityMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
        private readonly IdentitySharedService _identitySharedService;

        public IdentityMiddlewareResultHandler(IdentitySharedService identitySharedService)
        {
            _identitySharedService = identitySharedService;
        }

        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                //身份验证
                var subjectId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var sessionId = context.User.FindFirst(x => x.Type == AdminConsts.SessionId)?.Value;

                var requestToken = context.Request.Headers["Authorization"].ToString().Replace(JwtBearerDefaults.AuthenticationScheme, "").Trim();
                var isValid = await _identitySharedService.CheckTokenAsync(subjectId!, sessionId!, requestToken);
                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new AppResponse<bool>(ErrorCode.NoAuth, "身份信息已过期，请重新登录").SetData(false));
                    return;
                }

                //检查权限
                var endpoint = context.GetEndpoint();
                var auth = endpoint?.Metadata.GetMetadata<HasPermissionAttribute>();
                var mustMain = endpoint?.Metadata.GetMetadata<MustMainPowerAttribute>();
                var hasPower = true;
                if (mustMain != null)
                {
                    hasPower = await _identitySharedService.UserIsFromMainDbAsync(subjectId!);
                }
                if (hasPower && auth != null)
                {
                    hasPower = await _identitySharedService.CheckPermissionAsync(subjectId!, auth.Code);
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