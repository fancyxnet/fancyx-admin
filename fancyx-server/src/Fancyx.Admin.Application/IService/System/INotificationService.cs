using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface INotificationService : IScopedDependency
    {
        /// <summary>
        /// 添加通知
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AddNotificationAsync(AddOrUpdateNotificationRequest req);

        /// <summary>
        /// 通知列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<NotificationItem>> GetNotificationListAsync(GetNotificationListRequest req);

        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdateNotificationAsync(AddOrUpdateNotificationRequest req);

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteNotificationAsync(long[] ids);
    }
}