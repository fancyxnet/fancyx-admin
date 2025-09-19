using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.Organization
{
    public interface IPositionGroupService : IScopedDependency
    {
        /// <summary>
        /// 新增职位分组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddPositionGroupAsync(PositionGroupDto dto);

        /// <summary>
        /// 职位分组分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<PositionGroupListDto>> GetPositionGroupListAsync(PositionGroupQueryDto dto);

        /// <summary>
        /// 修改职位分组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdatePositionGroupAsync(PositionGroupDto dto);

        /// <summary>
        /// 删除职位分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePositionGroupAsync(Guid id);
    }
}