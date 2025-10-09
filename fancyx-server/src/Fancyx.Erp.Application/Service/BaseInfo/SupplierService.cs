using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.BaseInfo
{
    public class SupplierService : ISupplierService
    {
        public Task AddSupplierAsync(SupplierDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSupplierAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<SupplierListDto>> GetSupplierListAsync(SupplierQueryDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSupplierAsync(SupplierDto dto)
        {
            throw new NotImplementedException();
        }
    }
}