using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
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
        public async Task<AppResponse<bool>> AddNotificationAsync([FromBody] AddOrUpdateNotificationRequest req)
        {
            await _notificationService.AddNotificationAsync(req);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Sys.Notification.List")]
        public async Task<AppResponse<PagedResult<NotificationItem>>> GetNotificationListAsync([FromQuery] GetNotificationListRequest req)
        {
            var data = await _notificationService.GetNotificationListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Sys.Notification.Update")]
        public async Task<AppResponse<bool>> UpdateNotificationAsync([FromBody] AddOrUpdateNotificationRequest req)
        {
            await _notificationService.UpdateNotificationAsync(req);
            return Result.Ok();
        }

        [HttpDelete("BatchDelete")]
        [HasPermission("Sys.Notification.Delete")]
        public async Task<AppResponse<bool>> DeleteNotificationAsync([FromBody] long[] ids)
        {
            await _notificationService.DeleteNotificationAsync(ids);
            return Result.Ok();
        }

        [HttpGet("{id}")]
        [HasPermission("Sys.Notification.List")]
        public async Task<AppResponse<NotificationItem>> GetNotificationAsync([FromRoute] long id)
        {
            var data = await _notificationService.GetNotificationAsync(id);
            return Result.Data(data);
        }
    }
}