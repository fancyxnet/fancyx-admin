using Fancyx.Admin.Application.IService.System.LogManagement;
using Fancyx.Admin.Application.IService.System.LogManagement.Models;
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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        public async Task<AppResponse<PagedResult<BusinessLogItem>>> GetBusinessLogListAsync([FromQuery] GetBusinessLogListRequest req)
        {
            var data = await _businessLogService.GetBusinessLogListAsync(req);
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