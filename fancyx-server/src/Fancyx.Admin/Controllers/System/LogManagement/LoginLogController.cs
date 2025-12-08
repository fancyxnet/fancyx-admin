using Fancyx.Admin.Application.IService.System.LogManagement;
using Fancyx.Admin.Application.IService.System.LogManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.System.LogManagement
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginLogController : ControllerBase
    {
        private readonly ILoginLogService _loginLogService;

        public LoginLogController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        /// <summary>
        /// 登录日志分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetLoginLogList")]
        public async Task<AppResponse<PagedResult<LoginLogItem>>> GetLoginLogListAsync([FromQuery] GetLoginLogListRequest req)
        {
            var data = await _loginLogService.GetLoginLogListAsync(req);
            return Result.Data(data);
        }
    }
}