using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface IWarehouseService : IScopedDependency
    {
        Task AddWarehouseAsync(StoreHouseDto dto);
        Task<PagedResult<StoreHouseListDto>> GetWarehouseListAsync(StoreHouseQueryDto dto);
        Task UpdateWarehouseAsync(StoreHouseDto dto);
        Task DeleteWarehouseAsync(long id);
    }
}