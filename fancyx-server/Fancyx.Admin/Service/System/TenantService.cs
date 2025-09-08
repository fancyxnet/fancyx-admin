using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Repository;
using Fancyx.Repository.Entities.System;

namespace Fancyx.Admin.Service.System
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<TenantDO> _tenantRepository;

        public TenantService(IRepository<TenantDO> tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task AddTenantAsync(TenantDto dto)
        {
            if (await _tenantRepository.AnyAsync(x => x.TenantId.ToLower() == dto.TenantId.ToLower()))
            {
                throw new BusinessException($"租户标识[{dto.TenantId}]已存在");
            }

            var entity = new TenantDO()
            {
                Name = dto.Name,
                TenantId = dto.TenantId,
                Remark = dto.Remark,
                Domain = dto.Domain,
            };
            await _tenantRepository.InsertAsync(entity);
        }

        public async Task DeleteTenantAsync(Guid tenantId)
        {
            await _tenantRepository.DeleteAsync(x => x.Id == tenantId);
        }

        public async Task<PagedResult<TenantResultDto>> GetTenantListAsync(TenantSearchDto dto)
        {
            var resp = await _tenantRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Keyword), x => x.Name.Contains(dto.Keyword!) || x.TenantId.Contains(dto.Keyword!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new TenantResultDto { CreationTime = x.CreationTime, Domain = x.Domain, Id = x.Id, LastModificationTime = x.LastModificationTime, Name = x.Name, Remark = x.Remark, TenantId = x.TenantId })
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<TenantResultDto>(dto)
            {
                TotalCount = resp.Total,
                Items = resp.Items
            };
        }

        public async Task UpdateTenantAsync(TenantDto dto)
        {
            var entity = await _tenantRepository.GetAsync(x => x.Id == dto.Id) ?? throw new EntityNotFoundException();

            var tenantIdLower = dto.TenantId.ToLower();
            if (await _tenantRepository.AnyAsync(x => x.TenantId.ToLower() == tenantIdLower)
                && !tenantIdLower.Equals(entity.TenantId, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new BusinessException($"租户标识[{dto.TenantId}]已存在");
            }

            entity.Name = dto.Name;
            entity.TenantId = dto.TenantId;
            entity.Remark = dto.Remark;
            entity.Domain = dto.Domain;

            await _tenantRepository.UpdateAsync(entity);
        }
    }
}