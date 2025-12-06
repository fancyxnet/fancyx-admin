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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Sys.Role.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddRoleAsync([FromBody] AddOrUpdateRoleRequest dto)
        {
            await _roleService.AddRoleAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 角色分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.Role.List")]
        public async Task<AppResponse<PagedResult<RoleItem>>> GetRoleListAsync([FromQuery] GetRoleListRequest dto)
        {
            var data = await _roleService.GetRoleListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.Role.Update")]
        [ApiAccessLog(operateName: "修改角色", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateRoleAsync([FromBody] AddOrUpdateRoleRequest dto)
        {
            await _roleService.UpdateRoleAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id:long}")]
        [HasPermission("Sys.Role.Delete")]
        [ApiAccessLog(operateName: "删除角色", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteRoleAsync(long id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 分配菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AssignMenu")]
        [HasPermission("Sys.Role.AssignMenu")]
        [ApiAccessLog(operateName: "分配菜单权限", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> AssignMenuAsync([FromBody] AssignMenuRequest dto)
        {
            await _roleService.AssignMenuAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("Options")]
        public async Task<AppResponse<List<AppOption>>> GetRoleOptionsAsync()
        {
            var data = await _roleService.GetRoleOptionsAsync();
            return Result.Data(data);
        }

        /// <summary>
        /// 获取指定角色菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Menus/{id:long}")]
        public async Task<AppResponse<long[]>> GetRoleMenuIdsAsync(long id)
        {
            var data = await _roleService.GetRoleMenuIdsAsync(id);
            return Result.Data(data);
        }

        /// <summary>
        /// 获取角色部门权限编码
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        [HttpGet("GetRoleDeptPowerInfo")]
        public async Task<AppResponse<dynamic>> GetRoleDeptPowerInfoAsync(long roleId)
        {
            var data = await _roleService.GetRoleDeptPowerInfoAsync(roleId);
            return Result.Data<dynamic>(new { powerInfo = data.Item1, deptOptions = data.Item2 });
        }

        /// <summary>
        /// 分配角色数据权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AssignDataScope")]
        [HasPermission("Sys.Role.AssignDataScope")]
        public async Task<AppResponse<bool>> AssignDataScopeAsync([FromBody] AssignDataScopeRequest dto)
        {
            await _roleService.AssignDataScopeAsync(dto);
            return Result.Ok();
        }
    }
}