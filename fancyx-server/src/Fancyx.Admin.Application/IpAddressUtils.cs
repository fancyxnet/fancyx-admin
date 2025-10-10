using Fancyx.Utils;
using IP2Region.Net.XDB;
using System.Net;

namespace Fancyx.Admin.Application
{
    public class IpAddressUtils
    {
        public static string? ResolveAddress(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip) || !HttpUtils.IsValidIP(ip)) return null;

            //拿到地址示例：中国|0|重庆|重庆市|移动
            var address = new Searcher(CachePolicy.Content, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ip2region.xdb")).Search(ip!);
            if (string.IsNullOrWhiteSpace(address)) return string.Empty;
            if (address.Contains("0|0|0"))
            {
                return "未知";
            }
            string[] strs = address.Split('|');
            if (strs.Length >= 4)
            {
                return string.Concat(strs[0], strs[2], strs[3]);
            }
            else if (strs.Length == 1)
            {
                return strs[0];
            }
            return string.Empty;
        }
    }
}