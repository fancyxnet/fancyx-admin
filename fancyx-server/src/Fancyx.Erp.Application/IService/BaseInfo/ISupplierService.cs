using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface ISupplierService
    {
        Task AddSupplierAsync(SupplierDto dto);
        Task<PagedResult<SupplierListDto>> GetSupplierListAsync(SupplierQueryDto dto);
        Task UpdateSupplierAsync(SupplierDto dto);
        Task DeleteSupplierAsync(long id);
    }
}