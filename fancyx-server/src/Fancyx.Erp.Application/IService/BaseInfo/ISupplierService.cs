using Cracker.AspNetCore.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface ISupplierService : IScopedDependency
    {
        Task AddSupplierAsync(AddOrUpdateSupplierRequest req);
        Task<PagedResult<SupplierItem>> GetSupplierListAsync(GetSupplierListRequest req);
        Task UpdateSupplierAsync(AddOrUpdateSupplierRequest req);
        Task DeleteSupplierAsync(long id);
    }
}