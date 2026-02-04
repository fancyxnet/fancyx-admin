using Fancyx.Admin.Application.IService.Monitor.Models;
using Cracker.AspNetCore.Interfaces;

namespace Fancyx.Admin.Application.IService.Monitor
{
    public interface IOnlineUserService : IScopedDependency
    {
        /// <summary>
        /// 在线用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<OnlineUserItem>> GetOnlineUserListAsync(GetOnlineUserListRequest req);

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task LogoutAsync(string key);
    }
}