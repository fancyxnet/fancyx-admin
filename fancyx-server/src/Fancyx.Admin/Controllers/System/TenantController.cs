using DotNetCore.CAP;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger;
using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IMenuService _menuService;
        private readonly ICapPublisher _capPublisher;

        public TenantController(ITenantService tenantService, IMenuService menuService, ICapPublisher capPublisher)
        {
            _tenantService = tenantService;
            _menuService = menuService;
            _capPublisher = capPublisher;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Tenant.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        [ApiAccessLog(operateName: "添加租户", operateType: [OperateType.Create], reponseEnable: true)]
        public async Task<AppResponse<bool>> AddTenantAsync([FromBody] AddOrUpdateTenantRequest req)
        {
            await _tenantService.AddTenantAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.Tenant.List")]
        public async Task<AppResponse<PagedResult<TenantItem>>> GetTenantListAsync([FromQuery] GetTenantListRequest req)
        {
            var data = await _tenantService.GetTenantListAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.Tenant.Update")]
        [ApiAccessLog(operateName: "修改租户", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateTenantAsync([FromBody] AddOrUpdateTenantRequest req)
        {
            await _tenantService.UpdateTenantAsync(req);
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
        public async Task<AppResponse<bool>> AssignTenantMenuAsync([FromBody] AssignTenantMenuRequest req)
        {
            await _tenantService.AssignTenantMenuAsync(req);
            return Result.Ok();
        }

        [HttpGet("GetTenantMenuIds/{id}")]
        [HasPermission("Sys.Tenant.AssignTenantMenu")]
        public async Task<AppResponse<List<long>>> GetTenantMenuIdsAsync(string id)
        {
            var data = await _tenantService.GetTenantMenuIdsAsync(id);
            return Result.Data(data);
        }

        /// <summary>
        /// 获取菜单组成的选项树（全部，不含租户菜单过滤）
        /// </summary>
        /// <returns></returns>
        [HttpGet("MenuOptions")]
        [HasPermission("Sys.Tenant.MenuOptions")]
        public async Task<AppResponse<Dictionary<string, object>>> GetMenuOptionsAsync(bool onlyMenu, string? keyword)
        {
            var (keys, tree) = await _menuService.GetMenuOptionsAsync(onlyMenu, keyword, true);
            return Result.Data(new Dictionary<string, object> { ["keys"] = keys, ["tree"] = tree });
        }

        [HttpPost("CreateTenantAccount")]
        [HasPermission("Sys.Tenant.CreateTenantAccount")]
        public async Task<AppResponse<TenantAccountInfo>> CreateTenantAccount([FromBody] CreateTenantAccountRequest req)
        {
            var data = await _tenantService.CreateTenantAccountAsync(req);
            return Result.Data(data);
        }
    }
}