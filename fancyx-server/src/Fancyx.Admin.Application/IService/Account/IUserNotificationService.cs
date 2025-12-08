using Fancyx.Admin.Application.IService.Account.Models;

namespace Fancyx.Admin.Application.IService.Account
{
    public interface IUserNotificationService
    {
        Task<PagedResult<UserNotificationItem>> GetMyNotificationListAsync(GetMyNotificationListRequest req);

        Task ReadedAsync(long[] ids);

        Task<UserNotificationNavbarInfo> GetMyNotificationNavbarInfoAsync();
    }
}
