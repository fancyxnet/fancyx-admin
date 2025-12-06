using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Monitor
{
    public interface IOnlineUserService : IScopedDependency
    {
        Task<List<OnlineUserItem>> GetOnlineUserListAsync(GetOnlineUserListRequest dto);

        Task LogoutAsync(string key);
    }
}