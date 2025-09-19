namespace Fancyx.Shared.Consts;

/// <summary>
/// 错误码
/// </summary>
public static class ErrorCode
{
    /// <summary>
    /// 成功
    /// </summary>
    public const string Success = "0";

    /// <summary>
    /// 失败
    /// </summary>
    public const string Fail = "-1";

    /// <summary>
    /// 未登录
    /// </summary>
    public const string NoAuth = "401";

    /// <summary>
    /// 权限不足
    /// </summary>
    public const string Forbidden = "403";

    /// <summary>
    /// 限流错误码
    /// </summary>
    public const string ApiLimit = "429";
}