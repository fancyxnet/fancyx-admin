using Microsoft.AspNetCore.Http;

namespace Fancyx.Shared;

public static class HttpContextUtils
{
    public static string? GetIp(HttpContext context)
    {
        var header = context.Request.Headers;
        string? ip;
        if (header.TryGetValue("X-Real-IP", out var realIp))
        {
            ip = realIp;
        }
        else if (header.TryGetValue("X-Forwarded-For", out var forwardFor))
        {
            ip = forwardFor;
        }
        else
        {
            ip = context.Connection.RemoteIpAddress?.ToString();
        }
        return ip;
    }
}
