using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface IStoreHouseService : IScopedDependency
    {
        Task AddStoreHouseAsync(StoreHouseDto dto);
        Task<PagedResult<StoreHouseListDto>> GetStoreHouseListAsync(StoreHouseQueryDto dto);
        Task UpdateStoreHouseAsync(StoreHouseDto dto);
        Task DeleteStoreHouseAsync(long id);
    }
}