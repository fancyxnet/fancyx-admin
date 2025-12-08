using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface IWarehouseService : IScopedDependency
    {
        Task AddWarehouseAsync(AddOrUpdateWarehouseRequest req);
        Task<PagedResult<WarehouseItem>> GetWarehouseListAsync(GetWarehouseListRequest req);
        Task UpdateWarehouseAsync(AddOrUpdateWarehouseRequest req);
        Task DeleteWarehouseAsync(long id);

        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Warehouse> GetWarehouseAsync(long id);
    }
}