using Fancyx.Admin.Application.IService.Organization.Models;
using Cracker.AspNetCore.Interfaces;

namespace Fancyx.Admin.Application.IService.Organization
{
    public interface IDeptService : IScopedDependency
    {
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddDeptAsync(AddOrUpdateDeptRequest req);

        /// <summary>
        /// 部门树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<DeptItem>> GetDeptListAsync(GetDeptListRequest req);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateDeptAsync(AddOrUpdateDeptRequest req);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteDeptAsync(long id);

        /// <summary>
        /// 获取部门简单信息列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<DeptSimpleInfo>> GetDeptSimpleInfosAsync(string? keyword);

        /// <summary>
        /// 部门详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeptItem> GetDeptAsync(long id);
    }
}