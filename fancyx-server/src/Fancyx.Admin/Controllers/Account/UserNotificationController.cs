using Fancyx.Admin.IService.Account;
using Fancyx.Admin.IService.Account.Dtos;
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
        public async Task<AppResponse<PagedResult<UserNotificationListDto>>> GetMyNotificationListAsync([FromQuery] UserNotificationQueryDto dto)
        {
            var data = await _userNotificationService.GetMyNotificationListAsync(dto);
            return Result.Data(data);
        }

        [HttpPut("Readed")]
        public async Task<AppResponse<bool>> ReadedAsync([FromBody] Guid[] ids)
        {
            await _userNotificationService.ReadedAsync(ids);
            return Result.Ok();
        }

        [HttpGet("MyNotificationNavbarInfo")]
        public async Task<AppResponse<UserNotificationNavbarDto>> GetMyNotificationNavbarInfoAsync()
        {
            var data = await _userNotificationService.GetMyNotificationNavbarInfoAsync();
            return Result.Data(data);
        }
    }
}