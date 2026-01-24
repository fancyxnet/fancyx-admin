namespace Fancyx.Core.AutoInject;

/// <summary>
/// 只读当前请求作用域的服务提供器
/// </summary>
public class ServiceScopeAccessor
{
    private static readonly AsyncLocal<IServiceProvider> _currentScope = new();

    public static IServiceProvider Current => _currentScope.Value!;

    internal static void Set(IServiceProvider provider) => _currentScope.Value = provider;
}
