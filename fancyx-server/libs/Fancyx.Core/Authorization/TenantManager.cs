namespace Fancyx.Core.Authorization
{
    public class TenantManager
    {
        private static AsyncLocal<string> asyncLocal = null!;

        public static string? Current
        {
            get => asyncLocal?.Value;
        }

        public static void SetCurrent(string tenant)
        {
            asyncLocal ??= new AsyncLocal<string>();
            asyncLocal.Value = tenant;
        }
    }
}