using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Organization
{
    public interface IPositionService : IScopedDependency
    {
        /// <summary>
        /// 新增职位
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddPositionAsync(AddOrUpdatePositionRequest req);

        /// <summary>
        /// 职位分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<PositionItem>> GetPositionListAsync(GetPositionListRequest req);

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdatePositionAsync(AddOrUpdatePositionRequest req);

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePositionAsync(long id);

        /// <summary>
        /// 职位分组+职位树
        /// </summary>
        /// <returns></returns>
        Task<List<AppOptionTree>> GetPositionTreeOptionAsync();
    }
}