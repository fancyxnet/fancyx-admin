using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Organization
{
    public interface IPositionGroupService : IScopedDependency
    {
        /// <summary>
        /// 新增职位分组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddPositionGroupAsync(AddOrUpdatePositionGroupRequest req);

        /// <summary>
        /// 职位分组分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<PositionGroupItem>> GetPositionGroupListAsync(GetPositionGroupListRequest req);

        /// <summary>
        /// 修改职位分组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdatePositionGroupAsync(AddOrUpdatePositionGroupRequest req);

        /// <summary>
        /// 删除职位分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePositionGroupAsync(long id);

        /// <summary>
        /// 职位分组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PositionGroupItem> GetPositionAsync(long id);
    }
}