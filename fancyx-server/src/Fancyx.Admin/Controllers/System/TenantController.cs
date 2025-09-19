using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [MustMainPower]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService tenantService;

        public TenantController(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Tenant.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        [ApiAccessLog(operateName: "添加租户", operateType: [OperateType.Create], reponseEnable: true)]
        public async Task<AppResponse<bool>> AddTenantAsync([FromBody] TenantDto dto)
        {
            await tenantService.AddTenantAsync(dto);
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
            var data = await tenantService.GetTenantListAsync(dto);
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
            await tenantService.UpdateTenantAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id:Guid}")]
        [HasPermission("Sys.Tenant.Delete")]
        [ApiAccessLog(operateName: "删除租户", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteTenantAsync(Guid id)
        {
            await tenantService.DeleteTenantAsync(id);
            return Result.Ok();
        }
    }
}