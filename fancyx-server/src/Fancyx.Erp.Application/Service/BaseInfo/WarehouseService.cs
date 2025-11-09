using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.BaseInfo
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IRepository<Warehouse> storeHouseRepository, IMapper mapper)
        {
            _warehouseRepository = storeHouseRepository;
            _mapper = mapper;
        }

        public async Task AddWarehouseAsync(StoreHouseDto dto)
        {
            var codeIsExist = await _warehouseRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var storeHouse = new Warehouse()
            {
                Code = dto.Code,
                Name = dto.Name,
                Remark = dto.Remark,
                IsEnabled = dto.IsEnabled
            };
            await _warehouseRepository.InsertAsync(storeHouse);
        }

        public async Task DeleteWarehouseAsync(long id)
        {
            await _warehouseRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<StoreHouseListDto>> GetWarehouseListAsync(StoreHouseQueryDto dto)
        {
            var resp = await _warehouseRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<StoreHouseListDto>(resp.Total, _mapper.Map<List<StoreHouseListDto>>(resp.Items));
        }

        public async Task UpdateWarehouseAsync(StoreHouseDto dto)
        {
            var storeHouse = await _warehouseRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = storeHouse.Code != dto.Code && await _warehouseRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            storeHouse.Code = dto.Code;
            storeHouse.Name = dto.Name;
            storeHouse.Remark = dto.Remark;
            storeHouse.IsEnabled = dto.IsEnabled;
            await _warehouseRepository.UpdateAsync(storeHouse);
        }

        public async Task<Warehouse> GetWarehouseAsync(long id)
        {
            var entity = await _warehouseRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return entity;
        }
    }
}