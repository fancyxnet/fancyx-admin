using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Shared.WebApi.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Monitor
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorLogController : ControllerBase
    {
        private readonly IMonitorLogService _monitorLogService;

        public MonitorLogController(IMonitorLogService monitorLogService)
        {
            _monitorLogService = monitorLogService;
        }

        /// <summary>
        /// API访问日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("ApiAccessLogList")]
        [HasPermission("Monitor.ApiAccessLogList")]
        public async Task<AppResponse<PagedResult<ApiAccessLogItem>>> GetApiAccessLogListAsync([FromQuery] GetApiAccessLogListRequest dto)
        {
            var data = await _monitorLogService.GetApiAccessLogListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 异常日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("ExceptionLogList")]
        [HasPermission("Monitor.ExceptionLogList")]
        public async Task<AppResponse<PagedResult<ExceptionLogItem>>> GetExceptionLogListAsync([FromQuery] GetExceptionLogListRequest dto)
        {
            var data = await _monitorLogService.GetExceptionLogListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 标记异常已处理
        /// </summary>
        /// <param name="exceptionId">异常日志ID</param>
        /// <returns></returns>
        [HttpPost("HandleException")]
        [HasPermission("Monitor.ExceptionLog.HandleException")]
        public async Task<AppResponse<bool>> HandleExceptionAsync(long exceptionId)
        {
            await _monitorLogService.HandleExceptionAsync(exceptionId);
            return Result.Ok();
        }
    }
}