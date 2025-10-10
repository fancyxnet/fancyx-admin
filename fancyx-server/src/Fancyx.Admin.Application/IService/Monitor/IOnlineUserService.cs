using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Monitor
{
    public interface IOnlineUserService : IScopedDependency
    {
        Task<PagedResult<OnlineUserResultDto>> GetOnlineUserListAsync(OnlineUserSearchDto dto);

        Task LogoutAsync(string key);
    }
}