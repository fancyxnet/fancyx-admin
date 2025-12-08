using Fancyx.Admin.Application.IService.Monitor.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Monitor
{
    public interface IOnlineUserService : IScopedDependency
    {
        Task<List<OnlineUserItem>> GetOnlineUserListAsync(GetOnlineUserListRequest req);

        Task LogoutAsync(string key);
    }
}