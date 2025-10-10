using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;
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

        public async Task AddSupplierAsync(SupplierDto dto)
        {
            var codeIsExist = await _supplierRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var supplier = new Supplier()
            {
                Code = dto.Code,
                Name = dto.Name,
                Remark = dto.Remark,
                IsEnabled = dto.IsEnabled
            };
            await _supplierRepository.InsertAsync(supplier);
        }

        public async Task DeleteSupplierAsync(long id)
        {
            await _supplierRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<SupplierListDto>> GetSupplierListAsync(SupplierQueryDto dto)
        {
            var resp = await _supplierRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<SupplierListDto>(resp.Total, _mapper.Map<List<SupplierListDto>>(resp.Items));
        }

        public async Task UpdateSupplierAsync(SupplierDto dto)
        {
            var supplier = await _supplierRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = supplier.Code != dto.Code && await _supplierRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            supplier.Code = dto.Code;
            supplier.Name = dto.Name;
            supplier.Remark = dto.Remark;
            supplier.IsEnabled = dto.IsEnabled;
            await _supplierRepository.UpdateAsync(supplier);
        }
    }
}