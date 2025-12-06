using Fancyx.Admin.Application.IService.Organization.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Organization
{
    public interface IPositionService : IScopedDependency
    {
        /// <summary>
        /// 新增职位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddPositionAsync(AddPositionRequest dto);

        /// <summary>
        /// 职位分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<PositionItem>> GetPositionListAsync(GetPositionListRequest dto);

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdatePositionAsync(AddPositionRequest dto);

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