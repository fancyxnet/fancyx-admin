using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Dtos;

namespace Fancyx.Admin.Controllers.Monitor
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OnlineUserController : ControllerBase
    {
        private readonly IOnlineUserService _onlineUserService;

        public OnlineUserController(IOnlineUserService onlineUserService)
        {
            _onlineUserService = onlineUserService;
        }

        /// <summary>
        /// 在线用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission("Monitor.OnlineUser")]
        [ApiAccessLog(operateName: "在线用户列表", operateType: [OperateType.Query])]
        public async Task<AppResponse<List<OnlineUserResultDto>>> GetOnlineUserListAsync([FromQuery] OnlineUserSearchDto dto)
        {
            var data = await _onlineUserService.GetOnlineUserListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 注销用户会话
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission("Monitor.Logout")]
        [ApiAccessLog(operateName: "注销用户会话", operateType: [OperateType.Delete])]
        public async Task<AppResponse<bool>> LogoutAsync(string key)
        {
            await _onlineUserService.LogoutAsync(key);
            return Result.Ok();
        }
    }
}