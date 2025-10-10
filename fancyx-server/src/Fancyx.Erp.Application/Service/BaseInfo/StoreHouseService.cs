using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.BaseInfo
{
    public class StoreHouseService : IStoreHouseService
    {
        private readonly IRepository<StoreHouse> _storeHouseRepository;
        private readonly IMapper _mapper;

        public StoreHouseService(IRepository<StoreHouse> storeHouseRepository, IMapper mapper)
        {
            _storeHouseRepository = storeHouseRepository;
            _mapper = mapper;
        }

        public async Task AddStoreHouseAsync(StoreHouseDto dto)
        {
            var codeIsExist = await _storeHouseRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var storeHouse = new StoreHouse()
            {
                Code = dto.Code,
                Name = dto.Name,
                Remark = dto.Remark,
                IsEnabled = dto.IsEnabled
            };
            await _storeHouseRepository.InsertAsync(storeHouse);
        }

        public async Task DeleteStoreHouseAsync(long id)
        {
            await _storeHouseRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<StoreHouseListDto>> GetStoreHouseListAsync(StoreHouseQueryDto dto)
        {
            var resp = await _storeHouseRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<StoreHouseListDto>(resp.Total, _mapper.Map<List<StoreHouseListDto>>(resp.Items));
        }

        public async Task UpdateStoreHouseAsync(StoreHouseDto dto)
        {
            var storeHouse = await _storeHouseRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = storeHouse.Code != dto.Code && await _storeHouseRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            storeHouse.Code = dto.Code;
            storeHouse.Name = dto.Name;
            storeHouse.Remark = dto.Remark;
            storeHouse.IsEnabled = dto.IsEnabled;
            await _storeHouseRepository.UpdateAsync(storeHouse);
        }
    }
}