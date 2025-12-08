using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IRoleService : IScopedDependency
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddRoleAsync(AddOrUpdateRoleRequest req);

        /// <summary>
        /// 角色分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<RoleItem>> GetRoleListAsync(GetRoleListRequest req);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateRoleAsync(AddOrUpdateRoleRequest req);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(long id);

        /// <summary>
        /// 分配菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AssignMenuAsync(AssignMenuRequest req);

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        Task<List<AppOption>> GetRoleOptionsAsync();

        /// <summary>
        /// 获取指定角色菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<long[]> GetRoleMenuIdsAsync(long id);

        /// <summary>
        /// 获取角色部门权限编码
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<(RolePowerInfo, List<DeptTreeOption>)> GetRoleDeptPowerInfoAsync(long roleId);

        /// <summary>
        /// 分配角色数据权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AssignDataScopeAsync(AssignDataScopeRequest req);
    }
}