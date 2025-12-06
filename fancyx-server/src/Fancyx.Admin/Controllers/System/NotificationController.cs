using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Shared.WebApi.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Notification.Add")]
        public async Task<AppResponse<bool>> AddNotificationAsync([FromBody] AddOrUpdateNotificationRequest dto)
        {
            await _notificationService.AddNotificationAsync(dto);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Sys.Notification.List")]
        public async Task<AppResponse<PagedResult<NotificationItem>>> GetNotificationListAsync([FromQuery] GetNotificationListRequest dto)
        {
            var data = await _notificationService.GetNotificationListAsync(dto);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Sys.Notification.Update")]
        public async Task<AppResponse<bool>> UpdateNotificationAsync([FromBody] AddOrUpdateNotificationRequest dto)
        {
            await _notificationService.UpdateNotificationAsync(dto);
            return Result.Ok();
        }

        [HttpDelete("BatchDelete")]
        [HasPermission("Sys.Notification.Delete")]
        public async Task<AppResponse<bool>> DeleteNotificationAsync([FromBody] long[] ids)
        {
            await _notificationService.DeleteNotificationAsync(ids);
            return Result.Ok();
        }
    }
}