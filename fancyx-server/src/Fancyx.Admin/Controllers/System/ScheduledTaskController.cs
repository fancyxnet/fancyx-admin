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
    public class ScheduledTaskController : ControllerBase
    {
        private readonly IScheduledTaskService _scheduledTaskService;

        public ScheduledTaskController(IScheduledTaskService scheduledTaskService)
        {
            _scheduledTaskService = scheduledTaskService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.ScheduledTask.Add")]
        public async Task<AppResponse<bool>> AddAsync([FromBody] ScheduledTaskDto dto)
        {
            await _scheduledTaskService.AddAsync(dto);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Sys.ScheduledTask.List")]
        public async Task<AppResponse<PagedResult<ScheduledTaskListDto>>> GetListAsync([FromQuery] ScheduledTaskQueryDto dto)
        {
            var data = await _scheduledTaskService.GetListAsync(dto);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Sys.ScheduledTask.Update")]
        public async Task<AppResponse<bool>> UpdateAsync([FromBody] ScheduledTaskDto dto)
        {
            await _scheduledTaskService.UpdateAsync(dto);
            return Result.Ok();
        }

        [HttpDelete("Delete")]
        [HasPermission("Sys.ScheduledTask.Delete")]
        public async Task<AppResponse<bool>> DeleteAsync(string key)
        {
            await _scheduledTaskService.DeleteAsync(key);
            return Result.Ok();
        }

        [HttpGet("Log")]
        [HasPermission("Sys.ScheduledTask.Log")]
        public async Task<AppResponse<PagedResult<TaskExecutionLogListDto>>> GetExecutionLogListAsync([FromQuery] TaskExecutionLogQueryDto dto)
        {
            var data = await _scheduledTaskService.GetExecutionLogListAsync(dto);
            return Result.Data(data);
        }
    }
}