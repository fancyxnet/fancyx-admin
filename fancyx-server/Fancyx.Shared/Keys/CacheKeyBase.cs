using Fancyx.Core.Authorization;

namespace Fancyx.Shared.Keys
{
    public abstract class CacheKeyBase
    {
        public static string WithTenantPrefix(string key)
        {
            if (!string.IsNullOrEmpty(TenantManager.Current))
            {
                key = $"Tenant:{TenantManager.Current}:{key}";
            }
            return key;
        }
    }
}
