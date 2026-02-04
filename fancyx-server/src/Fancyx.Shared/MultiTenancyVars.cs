namespace Fancyx.Shared;

public static class MultiTenancyVars
{
    public static bool IsEnabled { get; private set; }

    public static void SetIsEnabled(bool flag)
    {
        IsEnabled = flag;
    }
}
