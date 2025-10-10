using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fancyx.Consul
{
    public class ServiceHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            // 这里添加实际健康检查逻辑
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
