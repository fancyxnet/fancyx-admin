using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;

namespace Fancyx.Admin.Application.Service.System
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<TenantMenu> _tenantMenuRepository;

        public TenantService(IRepository<Tenant> tenantRepository, IRepository<TenantMenu> tenantMenuRepository)
        {
            _tenantRepository = tenantRepository;
            _tenantMenuRepository = tenantMenuRepository;
        }

        public async Task AddTenantAsync(TenantDto dto)
        {
            if (await _tenantRepository.AnyAsync(x => x.Id.ToLower() == dto.TenantId.ToLower()))
            {
                throw new BusinessException($"租户标识[{dto.TenantId}]已存在");
            }

            var entity = new Tenant()
            {
                Name = dto.Name,
                Id = dto.TenantId,
                Remark = dto.Remark,
                Domain = dto.Domain,
            };
            await _tenantRepository.InsertAsync(entity);
        }

        public async Task DeleteTenantAsync(string tenantId)
        {
            await _tenantRepository.DeleteAsync(x => x.Id == tenantId);
        }

        public async Task<PagedResult<TenantResultDto>> GetTenantListAsync(TenantSearchDto dto)
        {
            var resp = await _tenantRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Keyword), x => x.Name.Contains(dto.Keyword!) || x.Id.Contains(dto.Keyword!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new TenantResultDto { CreationTime = x.CreationTime, Domain = x.Domain, LastModificationTime = x.LastModificationTime, Name = x.Name, Remark = x.Remark, TenantId = x.Id })
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<TenantResultDto>(dto)
            {
                TotalCount = resp.Total,
                Items = resp.Items
            };
        }

        public async Task UpdateTenantAsync(TenantDto dto)
        {
            var entity = await _tenantRepository.FindAsync(dto.TenantId) ?? throw new EntityNotFoundException();

            entity.Name = dto.Name;
            entity.Remark = dto.Remark;
            entity.Domain = dto.Domain;

            await _tenantRepository.UpdateAsync(entity);
        }

        [AsyncTransactional]
        public async Task AssignTenantMenuAsync(AssignTenantMenuDto dto)
        {
            await _tenantMenuRepository.DeleteAsync(x => x.TenantId == dto.TenantId);
            if (dto.MenuIds?.Length > 0)
            {
                var tenantMenus = dto.MenuIds.Select(id => new TenantMenu
                {
                    TenantId = dto.TenantId,
                    MenuId = id
                }).ToList();
                await _tenantMenuRepository.InsertManyAsync(tenantMenus);
            }
        }

        public Task<List<long>> GetTenantMenuIdsAsync(string id)
        {
            return _tenantMenuRepository.Where(x => x.TenantId == id).SelectToListAsync(x => x.MenuId);
        }
    }
}