using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface INotificationService : IScopedDependency
    {
        Task AddNotificationAsync(AddOrUpdateNotificationRequest req);

        Task<PagedResult<NotificationItem>> GetNotificationListAsync(GetNotificationListRequest req);

        Task UpdateNotificationAsync(AddOrUpdateNotificationRequest req);

        Task DeleteNotificationAsync(long[] ids);
    }
}