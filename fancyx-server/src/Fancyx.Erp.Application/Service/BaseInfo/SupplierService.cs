using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.BaseInfo
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(IRepository<Supplier> supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task AddSupplierAsync(AddOrUpdateSupplierRequest req)
        {
            var codeIsExist = await _supplierRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var supplier = new Supplier()
            {
                Code = req.Code,
                Name = req.Name,
                Remark = req.Remark,
                IsEnabled = req.IsEnabled
            };
            await _supplierRepository.InsertAsync(supplier);
        }

        public async Task DeleteSupplierAsync(long id)
        {
            await _supplierRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<SupplierItem>> GetSupplierListAsync(GetSupplierListRequest req)
        {
            var resp = await _supplierRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.StartsWith(req.Name!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<SupplierItem>(resp.Total, _mapper.Map<List<SupplierItem>>(resp.Items));
        }

        public async Task UpdateSupplierAsync(AddOrUpdateSupplierRequest req)
        {
            var supplier = await _supplierRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = supplier.Code != req.Code && await _supplierRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            supplier.Code = req.Code;
            supplier.Name = req.Name;
            supplier.Remark = req.Remark;
            supplier.IsEnabled = req.IsEnabled;
            await _supplierRepository.UpdateAsync(supplier);
        }
    }
}