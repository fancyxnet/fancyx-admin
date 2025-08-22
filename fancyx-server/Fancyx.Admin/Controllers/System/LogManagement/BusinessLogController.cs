using Fancyx.Admin.IService.System.LogManagement;
using Fancyx.Admin.IService.System.LogManagement.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.System.LogManagement
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessLogController : ControllerBase
    {
        private readonly IBusinessLogService _businessLogService;

        public BusinessLogController(IBusinessLogService businessLogService)
        {
            _businessLogService = businessLogService;
        }

        /// <summary>
        /// 业务日志分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<AppResponse<PagedResult<BusinessLogListDto>>> GetBusinessLogListAsync([FromQuery] BusinessLogQueryDto dto)
        {
            var data = await _businessLogService.GetBusinessLogListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 获取所有业务类型选项
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("TypeOptions")]
        public async Task<AppResponse<List<AppOption>>> GetBusinessTypeOptionsAsync(string? type)
        {
            var data = await _businessLogService.GetBusinessTypeOptionsAsync(type);
            return Result.Data(data);
        }
    }
}