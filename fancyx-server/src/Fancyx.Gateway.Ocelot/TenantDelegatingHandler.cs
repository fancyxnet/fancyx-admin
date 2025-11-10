namespace Fancyx.Gateway.Ocelot
{
    public class TenantDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 从域名或Header解析租户ID
            const string tenantDomainHeaderKey = "X-Tenant-Domain";
            if (!request.Headers.Contains(tenantDomainHeaderKey))
            {
                string? domain = null;
                if (request.Headers.Contains("X-Forwarded-Host"))
                {
                    domain = request.Headers.GetValues("X-Forwarded-Host").FirstOrDefault();
                }
                else if (request.Headers.Contains("X-Original-Host"))
                {
                    domain = request.Headers.GetValues("X-Original-Host").FirstOrDefault();
                }
                else
                {
                    domain = request.Headers.Host ?? request.RequestUri?.Host;
                }
                if (!string.IsNullOrWhiteSpace(domain))
                {
                    request.Headers.Add(tenantDomainHeaderKey, domain);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
