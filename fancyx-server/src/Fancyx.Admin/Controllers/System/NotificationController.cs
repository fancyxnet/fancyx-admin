using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Notification.Add")]
        public async Task<AppResponse<bool>> AddNotificationAsync([FromBody] NotificationDto dto)
        {
            await notificationService.AddNotificationAsync(dto);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Sys.Notification.List")]
        public async Task<AppResponse<PagedResult<NotificationResultDto>>> GetNotificationListAsync([FromQuery] NotificationQueryDto dto)
        {
            var data = await notificationService.GetNotificationListAsync(dto);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Sys.Notification.Update")]
        public async Task<AppResponse<bool>> UpdateNotificationAsync([FromBody] NotificationDto dto)
        {
            await notificationService.UpdateNotificationAsync(dto);
            return Result.Ok();
        }

        [HttpDelete("BatchDelete")]
        [HasPermission("Sys.Notification.Delete")]
        public async Task<AppResponse<bool>> DeleteNotificationAsync([FromBody] Guid[] ids)
        {
            await notificationService.DeleteNotificationAsync(ids);
            return Result.Ok();
        }
    }
}