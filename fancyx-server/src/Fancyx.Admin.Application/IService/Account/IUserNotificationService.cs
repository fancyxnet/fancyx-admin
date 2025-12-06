using Fancyx.Admin.Application.IService.Account.Dtos;

namespace Fancyx.Admin.Application.IService.Account
{
    public interface IUserNotificationService
    {
        Task<PagedResult<UserNotificationItem>> GetMyNotificationListAsync(GetMyNotificationListRequest dto);

        Task ReadedAsync(long[] ids);

        Task<UserNotificationNavbarInfo> GetMyNotificationNavbarInfoAsync();
    }
}
