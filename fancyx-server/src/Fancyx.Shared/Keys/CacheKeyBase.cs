using Fancyx.Core.Authorization;

namespace Fancyx.Shared.Keys
{
    public abstract class CacheKeyBase
    {
        public static string WithTenantPrefix(string key)
        {
            if (!string.IsNullOrEmpty(TenantManager.Current))
            {
                key = $"tenant:{TenantManager.Current}:{key}";
            }
            return key;
        }

        public static string PureKey(string key) 
        {
            if (!string.IsNullOrEmpty(TenantManager.Current))
                return key.Replace($"tenant:{TenantManager.Current}:", "");
            return key;
        }
    }
}
