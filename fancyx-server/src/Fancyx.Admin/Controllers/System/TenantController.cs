using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.Application.IService.System;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Tenant.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        [ApiAccessLog(operateName: "添加租户", operateType: [OperateType.Create], reponseEnable: true)]
        public async Task<AppResponse<bool>> AddTenantAsync([FromBody] TenantDto dto)
        {
            await _tenantService.AddTenantAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.Tenant.List")]
        public async Task<AppResponse<PagedResult<TenantResultDto>>> GetTenantListAsync([FromQuery] TenantSearchDto dto)
        {
            var data = await _tenantService.GetTenantListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.Tenant.Update")]
        [ApiAccessLog(operateName: "修改租户", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateTenantAsync([FromBody] TenantDto dto)
        {
            await _tenantService.UpdateTenantAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [HasPermission("Sys.Tenant.Delete")]
        [ApiAccessLog(operateName: "删除租户", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteTenantAsync(string id)
        {
            await _tenantService.DeleteTenantAsync(id);
            return Result.Ok();
        }

        [HttpPost("AssignTenantMenu")]
        [HasPermission("Sys.Tenant.AssignTenantMenu")]
        public async Task<AppResponse<bool>> AssignTenantMenuAsync([FromBody] AssignTenantMenuDto dto)
        {
            await _tenantService.AssignTenantMenuAsync(dto);
            return Result.Ok();
        }

        [HttpGet("GetTenantMenuIds/{id}")]
        [HasPermission("Sys.Tenant.AssignTenantMenu")]
        public async Task<AppResponse<List<long>>> GetTenantMenuIdsAsync(string id)
        {
            var data = await _tenantService.GetTenantMenuIdsAsync(id);
            return Result.Data(data);
        }
    }
}