using Fancyx.Admin.Application.IService.Account.Dtos;

namespace Fancyx.Admin.Application.IService.Account
{
    public interface IUserNotificationService
    {
        Task<PagedResult<UserNotificationListDto>> GetMyNotificationListAsync(UserNotificationQueryDto dto);

        Task ReadedAsync(Guid[] ids);

        Task<UserNotificationNavbarDto> GetMyNotificationNavbarInfoAsync();
    }
}
