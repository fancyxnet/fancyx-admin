using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
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
            // 使用X-Tenant解析租户ID，是最开始方案，但无论通过Nginx配置还是Ocelot配置转发过来，都需要更新配置(Consul自动刷新)或重启才生效，内部Grpc也传递此请求头
            // 使用X-Tenant-Domain将原始域名通过Nginx和Ocelot转发过来，再从缓存中读取域名绑定的租户ID，这样可以无感解析租户，无需重启/刷新

            try
            {
                var checker = context.RequestServices.GetService<ITenantChecker>();
                if (checker == null)
                {
                    await next(context);
                    return;
                }
                if (context.Request.Headers.TryGetValue("X-Tenant", out var tenant) && !string.IsNullOrWhiteSpace(tenant))
                {
                    var tenantId = tenant.ToString();
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
                if (!IsGrpcRequest(context) && context.Request.Headers.TryGetValue("X-Tenant-Domain", out var domain) && !string.IsNullOrWhiteSpace(domain))
                {
                    var tenantId = await checker.GetTenantByDomainAsync(domain!);
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