using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface IWarehouseService : IScopedDependency
    {
        Task AddWarehouseAsync(StoreHouseDto dto);
        Task<PagedResult<StoreHouseListDto>> GetWarehouseListAsync(StoreHouseQueryDto dto);
        Task UpdateWarehouseAsync(StoreHouseDto dto);
        Task DeleteWarehouseAsync(long id);

        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Warehouse> GetWarehouseAsync(long id);
    }
}