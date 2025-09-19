using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.Monitor
{
    public interface IOnlineUserService : IScopedDependency
    {
        Task<PagedResult<OnlineUserResultDto>> GetOnlineUserListAsync(OnlineUserSearchDto dto);

        Task LogoutAsync(string key);
    }
}