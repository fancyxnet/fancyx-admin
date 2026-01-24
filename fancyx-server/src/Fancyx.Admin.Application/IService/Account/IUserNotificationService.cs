using Fancyx.Admin.Application.IService.Account.Models;

namespace Fancyx.Admin.Application.IService.Account
{
    public interface IUserNotificationService
    {
        /// <summary>
        /// 我的通知列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<UserNotificationItem>> GetMyNotificationListAsync(GetMyNotificationListRequest req);

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task ReadedAsync(List<long> ids);

        /// <summary>
        /// 我的通知导航栏信息
        /// </summary>
        /// <returns></returns>
        Task<UserNotificationNavbarInfo> GetMyNotificationNavbarInfoAsync();
    }
}
