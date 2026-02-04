using AutoMapper;
using Cracker.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entities;
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

        public async Task AddWarehouseAsync(AddOrUpdateWarehouseRequest req)
        {
            var codeIsExist = await _warehouseRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var storeHouse = new Warehouse()
            {
                Code = req.Code,
                Name = req.Name,
                Remark = req.Remark,
                IsEnabled = req.IsEnabled
            };
            await _warehouseRepository.InsertAsync(storeHouse);
        }

        public async Task DeleteWarehouseAsync(long id)
        {
            await _warehouseRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<WarehouseItem>> GetWarehouseListAsync(GetWarehouseListRequest req)
        {
            var resp = await _warehouseRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.StartsWith(req.Name!))
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<WarehouseItem>(resp.Total, _mapper.Map<List<WarehouseItem>>(resp.Items));
        }

        public async Task UpdateWarehouseAsync(AddOrUpdateWarehouseRequest req)
        {
            var storeHouse = await _warehouseRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = storeHouse.Code != req.Code && await _warehouseRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            storeHouse.Code = req.Code;
            storeHouse.Name = req.Name;
            storeHouse.Remark = req.Remark;
            storeHouse.IsEnabled = req.IsEnabled;
            await _warehouseRepository.UpdateAsync(storeHouse);
        }

        public async Task<Warehouse> GetWarehouseAsync(long id)
        {
            var entity = await _warehouseRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return entity;
        }
    }
}