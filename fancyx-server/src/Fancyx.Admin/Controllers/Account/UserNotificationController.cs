using Fancyx.Admin.Application.IService.Account;
using Fancyx.Admin.Application.IService.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserNotificationController : ControllerBase
    {
        private readonly IUserNotificationService _userNotificationService;

        public UserNotificationController(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }

        [HttpGet("MyNotificationList")]
        public async Task<AppResponse<PagedResult<UserNotificationItem>>> GetMyNotificationListAsync([FromQuery] GetMyNotificationListRequest req)
        {
            var data = await _userNotificationService.GetMyNotificationListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Readed")]
        public async Task<AppResponse<bool>> ReadedAsync([FromBody] List<long> ids)
        {
            await _userNotificationService.ReadedAsync(ids);
            return Result.Ok();
        }

        [HttpGet("MyNotificationNavbarInfo")]
        public async Task<AppResponse<UserNotificationNavbarInfo>> GetMyNotificationNavbarInfoAsync()
        {
            var data = await _userNotificationService.GetMyNotificationNavbarInfoAsync();
            return Result.Data(data);
        }
    }
}