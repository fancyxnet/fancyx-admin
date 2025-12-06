using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IRoleService : IScopedDependency
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddRoleAsync(AddOrUpdateRoleRequest dto);

        /// <summary>
        /// 角色分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<RoleItem>> GetRoleListAsync(GetRoleListRequest dto);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateRoleAsync(AddOrUpdateRoleRequest dto);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(long id);

        /// <summary>
        /// 分配菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AssignMenuAsync(AssignMenuRequest dto);

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
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AssignDataScopeAsync(AssignDataScopeRequest dto);
    }
}