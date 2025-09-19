namespace Fancyx.Core.Authorization
{
    public class UserManager
    {
        private static AsyncLocal<Guid> asyncLocal = null!;

        public static Guid? Current
        {
            get => asyncLocal?.Value;
        }

        public static void SetCurrent(Guid userId)
        {
            asyncLocal ??= new AsyncLocal<Guid>();
            asyncLocal.Value = userId;
        }
    }
}