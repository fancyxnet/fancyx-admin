using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fancyx.Core.Middlewares
{
    public class MultiTenancyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<MultiTenancyMiddleware> logger;

        public MultiTenancyMiddleware(RequestDelegate next, ILogger<MultiTenancyMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var hasTenantId = context.Request.Headers.TryGetValue("X-Tenant", out var tenant);
                var tenantId = tenant.ToString();
                var checker = context.RequestServices.GetService<ITenantChecker>();
                if (checker == null)
                {
                    await next(context);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(tenantId))
                {
                    if (!await checker.ExistTenantAsync(tenantId))
                    {
                        logger.LogWarning("租户{tenantId}不存在", tenantId);
                        context.Response.StatusCode = 403;
                        return;
                    }
                    context.Features.Set(new CurrentTenant(tenantId));
                    TenantManager.SetCurrent(tenantId);
                    await next(context);
                    return;
                }
                // TODO: Ocelot网关过来的域名都是localhost
                if (!IsGrpcRequest(context))
                {
                    var domain = context.Request.Host.Host;
                    tenantId = await checker.GetTenantByDomainAsync(domain);
                    if (!string.IsNullOrWhiteSpace(tenantId))
                    {
                        context.Features.Set(new CurrentTenant(tenantId));
                        TenantManager.SetCurrent(tenantId);
                    }
                }

                await next(context);
            }
            finally
            {
                TenantManager.SetCurrent("");
            }
        }

        public static bool IsGrpcRequest(HttpContext httpContext)
        {
            // 检查协议
            if (httpContext.Request.Protocol != "HTTP/2")
                return false;

            // 检查 Content-Type
            var contentType = httpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType))
                return false;

            // gRPC 的 Content-Type 通常是 "application/grpc" 或 "application/grpc+proto"
            return contentType.StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase);
        }
    }
}