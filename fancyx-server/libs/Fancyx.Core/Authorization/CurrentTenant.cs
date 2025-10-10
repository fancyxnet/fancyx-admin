using Fancyx.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Fancyx.Core.Authorization
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly string? _tenantId;
        public string? TenantId => _tenantId;

        public CurrentTenant(string? name)
        {
            _tenantId = name;
        }

        public static ICurrentTenant Parse(HttpContext? context)
        {
            return context?.Features.Get<CurrentTenant>() ?? new CurrentTenant(null);
        }
    }
}