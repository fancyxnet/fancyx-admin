using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System;

public interface IUserService : IScopedDependency
{
    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<long> AddUserAsync(AddUserRequest dto);

    /// <summary>
    /// 用户分页列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<PagedResult<UserItem>> GetUserListAsync(GetUserListRequest dto);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteUserAsync(long id);

    /// <summary>
    /// 分配角色
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<bool> AssignRoleAsync(AssignRoleRequest dto);

    /// <summary>
    /// 切换用户启用状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> SwitchUserEnabledStatusAsync(long id);

    /// <summary>
    /// 获取指定用户角色
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    Task<long[]> GetUserRoleIdsAsync(long uid);

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task ResetUserPasswordAsync(ResetUserPwdRequest dto);

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
    Task<List<UserItem>> ExportUserListAsync(GetUserListRequest dto);

    /// <summary>
    /// 编辑用户信息
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task UpdateUserAsync(UpdateUserRequest dto);

    /// <summary>
    /// 获取用户编辑信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDetails> GetUserEditInfoAsync(long id); 
}