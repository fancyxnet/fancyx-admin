using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.Shared.Keys;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Fancyx.Admin.Application.Service.System
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<TenantMenu> _tenantMenuRepository;
        private readonly IDatabase _redis;
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<RoleMenu> _roleMenuRepository;

        public TenantService(IRepository<Tenant> tenantRepository, IRepository<TenantMenu> tenantMenuRepository, IDatabase redis
            , IdentitySharedService identitySharedService, IRepository<User> userRepository, ICurrentUser currentUser, IRepository<RoleMenu> roleMenuRepository)
        {
            _tenantRepository = tenantRepository;
            _tenantMenuRepository = tenantMenuRepository;
            _redis = redis;
            _identitySharedService = identitySharedService;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _roleMenuRepository = roleMenuRepository;
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
                IsEnabled = dto.IsEnabled
            };
            await _tenantRepository.InsertAsync(entity);
            await _redis.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _redis.KeyDeleteAsync(SystemCacheKey.TenantDomains);
        }

        public async Task DeleteTenantAsync(string tenantId)
        {
            await _tenantRepository.DeleteAsync(x => x.Id == tenantId);
            await _redis.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _redis.KeyDeleteAsync(SystemCacheKey.TenantDomains);
            await this.DisabledTenantSubUserAsync(tenantId);
        }

        public async Task<PagedResult<TenantResultDto>> GetTenantListAsync(TenantSearchDto dto)
        {
            var resp = await _tenantRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Keyword), x => x.Name.Contains(dto.Keyword!) || x.Id.Contains(dto.Keyword!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new TenantResultDto { IsEnabled = x.IsEnabled, CreationTime = x.CreationTime, Domain = x.Domain, LastModificationTime = x.LastModificationTime, Name = x.Name, Remark = x.Remark, TenantId = x.Id })
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
            entity.IsEnabled = dto.IsEnabled;

            await _tenantRepository.UpdateAsync(entity);
            await _redis.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _redis.KeyDeleteAsync(SystemCacheKey.TenantDomains);

            if (!dto.IsEnabled)
            {
                await this.DisabledTenantSubUserAsync(dto.TenantId);
            }
        }

        [AsyncTransactional]
        public async Task AssignTenantMenuAsync(AssignTenantMenuDto dto)
        {
            await _tenantMenuRepository.DeleteAsync(x => x.TenantId == dto.TenantId);
            if (dto.MenuIds?.Length > 0)
            {
                // 找到本次移除的菜单ID，移除租户下所有角色已分配的对应菜单ID
                var existMenuIds = await _roleMenuRepository.Where(x => x.TenantId == dto.TenantId).Select(x => x.MenuId).ToListAsync();
                var removeMenuIds = existMenuIds.Where(x => !dto.MenuIds.Contains(x)).ToList();
                await _roleMenuRepository.DeleteAsync(x => removeMenuIds.Contains(x.MenuId));

                var tenantMenus = dto.MenuIds.Select(id => new TenantMenu
                {
                    TenantId = dto.TenantId,
                    MenuId = id
                }).ToList();
                await _tenantMenuRepository.InsertManyAsync(tenantMenus);
            }
            await _identitySharedService.DelUserPermissionCacheByTenantIdAsync(dto.TenantId);
        }

        public Task<List<long>> GetTenantMenuIdsAsync(string id)
        {
            return _tenantMenuRepository.Where(x => x.TenantId == id).SelectToListAsync(x => x.MenuId);
        }

        private async Task DisabledTenantSubUserAsync(string tenantId)
        {
            await _identitySharedService.DelUserPermissionCacheByTenantIdAsync(tenantId, true);
            // 禁用租户下所有账号
            await _userRepository.GetQueryable().IgnoreQueryFilters().Where(x => x.TenantId == tenantId && x.IsEnabled)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsEnabled, false)
                .SetProperty(e => e.LastModificationTime, DateTime.Now)
                .SetProperty(e => e.LastModifierId, _currentUser.Id));
        }
    }
}