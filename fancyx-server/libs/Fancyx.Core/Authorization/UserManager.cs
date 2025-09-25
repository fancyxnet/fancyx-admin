namespace Fancyx.Core.Authorization
{
    public class UserManager
    {
        private static AsyncLocal<long> asyncLocal = null!;

        public static long? Current
        {
            get => asyncLocal?.Value;
        }

        public static void SetCurrent(long userId)
        {
            asyncLocal ??= new AsyncLocal<long>();
            asyncLocal.Value = userId;
        }
    }
}