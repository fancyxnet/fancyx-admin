using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface INotificationService : IScopedDependency
    {
        Task AddNotificationAsync(NotificationDto dto);

        Task<PagedResult<NotificationResultDto>> GetNotificationListAsync(NotificationQueryDto dto);

        Task UpdateNotificationAsync(NotificationDto dto);

        Task DeleteNotificationAsync(Guid[] ids);
    }
}