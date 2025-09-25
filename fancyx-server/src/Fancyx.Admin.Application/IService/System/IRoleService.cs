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
        Task<bool> AddRoleAsync(RoleDto dto);

        /// <summary>
        /// 角色分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<RoleListDto>> GetRoleListAsync(RoleQueryDto dto);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateRoleAsync(RoleDto dto);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(Guid id);

        /// <summary>
        /// 分配菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AssignMenuAsync(AssignMenuDto dto);

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
        Task<Guid[]> GetRoleMenuIdsAsync(Guid id);

        /// <summary>
        /// 获取角色部门权限编码
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<(RolePowerInfoDto, List<DeptTreeOptionDto>)> GetRoleDeptPowerInfoAsync(Guid roleId);

        /// <summary>
        /// 分配角色数据权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AssignDataScopeAsync(AssignDataScopeDto dto);
    }
}