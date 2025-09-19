using Microsoft.AspNetCore.Http;
using System.Net;

namespace Fancyx.Utils
{
    public static class HttpUtils
    {
        public static string? GetBrowserByUA(string? userAgent)
        {
            var parser = UAParser.Parser.GetDefault().Parse(userAgent);
            return parser.String;
        }

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

        public static bool IsValidIP(string ipString)
        {
            return IPAddress.TryParse(ipString, out IPAddress? address) &&
                   address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }
    }
}