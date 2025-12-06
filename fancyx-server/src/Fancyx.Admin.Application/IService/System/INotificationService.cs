using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface INotificationService : IScopedDependency
    {
        Task AddNotificationAsync(AddOrUpdateNotificationRequest dto);

        Task<PagedResult<NotificationItem>> GetNotificationListAsync(GetNotificationListRequest dto);

        Task UpdateNotificationAsync(AddOrUpdateNotificationRequest dto);

        Task DeleteNotificationAsync(long[] ids);
    }
}