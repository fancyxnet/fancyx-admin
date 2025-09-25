using Fancyx.Admin.Application.IService.Organization.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Organization
{
    public interface IDeptService : IScopedDependency
    {
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddDeptAsync(DeptDto dto);

        /// <summary>
        /// 部门树形列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<DeptListDto>> GetDeptListAsync(DeptQueryDto dto);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateDeptAsync(DeptDto dto);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteDeptAsync(Guid id);

        /// <summary>
        /// 获取部门简单信息列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<DeptSimpleInfoDto>> GetDeptSimpleInfosAsync(string? keyword);
    }
}