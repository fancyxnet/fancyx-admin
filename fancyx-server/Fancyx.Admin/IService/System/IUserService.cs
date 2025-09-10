using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.System
{
    public interface IUserService : IScopedDependency
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Guid> AddUserAsync(UserDto dto);

        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<UserListDto>> GetUserListAsync(UserQueryDto dto);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(Guid id);

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AssignRoleAsync(AssignRoleDto dto);

        /// <summary>
        /// 切换用户启用状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SwitchUserEnabledStatusAsync(Guid id);

        /// <summary>
        /// 获取指定用户角色
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<Guid[]> GetUserRoleIdsAsync(Guid uid);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ResetUserPasswordAsync(ResetUserPwdDto dto);

        /// <summary>
        /// 用户简单信息查询
        /// </summary>
        /// <param name="keyword">账号/昵称</param>
        /// <returns></returns>
        Task<List<UserSimpleInfoDto>> GetUserSimpleInfosAsync(string? keyword);

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<UserListDto>> ExportUserListAsync(UserQueryDto dto);

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UserEditDto dto);
    }
}