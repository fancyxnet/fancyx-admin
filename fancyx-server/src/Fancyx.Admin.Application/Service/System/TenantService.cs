using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Cache;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Keys;
using Fancyx.SnowflakeId;
using Fancyx.Utils;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.System
{
    public class TenantService : ITenantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<TenantMenu> _tenantMenuRepository;
        private readonly ICacheClient _cache;
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<RoleMenu> _roleMenuRepository;
        private readonly FancyxDbContext _dbContext;

        public TenantService(IRepository<Tenant> tenantRepository, IRepository<TenantMenu> tenantMenuRepository, ICacheClient cache
            , IdentitySharedService identitySharedService, IRepository<User> userRepository, ICurrentUser currentUser, IRepository<RoleMenu> roleMenuRepository, FancyxDbContext dbContext)
        {
            _tenantRepository = tenantRepository;
            _tenantMenuRepository = tenantMenuRepository;
            _cache = cache;
            _identitySharedService = identitySharedService;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _roleMenuRepository = roleMenuRepository;
            _dbContext = dbContext;
        }

        public async Task AddTenantAsync(AddOrUpdateTenantRequest req)
        {
            if (await _tenantRepository.AnyAsync(x => x.Id.ToLower() == req.TenantId.ToLower()))
            {
                throw new BusinessException($"租户标识[{req.TenantId}]已存在");
            }
            if (await _tenantRepository.AnyAsync(x => x.Domain.ToLower() == req.Domain.ToLower()))
            {
                throw new BusinessException($"租户域名[{req.TenantId}]已存在");
            }

            var entity = new Tenant()
            {
                Name = req.Name,
                Id = req.TenantId,
                Remark = req.Remark,
                Domain = req.Domain.ToLowerInvariant(),
                IsEnabled = req.IsEnabled
            };
            await _tenantRepository.InsertAsync(entity);
            await _cache.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _cache.KeyDeleteAsync(SystemCacheKey.TenantDomains);
        }

        public async Task DeleteTenantAsync(string tenantId)
        {
            await _tenantRepository.DeleteAsync(x => x.Id == tenantId);
            await _tenantMenuRepository.DeleteAsync(x => x.TenantId == tenantId);
            await _cache.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _cache.KeyDeleteAsync(SystemCacheKey.TenantDomains);
            await this.DisabledTenantSubUserAsync(tenantId);
        }

        public async Task<PagedResult<TenantItem>> GetTenantListAsync(GetTenantListRequest req)
        {
            var resp = await _tenantRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Keyword), x => x.Name.Contains(req.Keyword!) || x.Id.Contains(req.Keyword!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new TenantItem { IsEnabled = x.IsEnabled, CreationTime = x.CreationTime, Domain = x.Domain, LastModificationTime = x.LastModificationTime, Name = x.Name, Remark = x.Remark, TenantId = x.Id })
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<TenantItem>(req)
            {
                TotalCount = resp.Total,
                Items = resp.Items
            };
        }

        public async Task UpdateTenantAsync(AddOrUpdateTenantRequest req)
        {
            var entity = await _tenantRepository.FindAsync(req.TenantId) ?? throw new EntityNotFoundException();

            var domain = req.Domain.ToLowerInvariant();
            if (entity.Domain != domain && await _tenantRepository.AnyAsync(x => x.Domain.ToLower() == domain))
            {
                throw new BusinessException($"租户域名[{req.TenantId}]已存在");
            }

            entity.Name = req.Name;
            entity.Remark = req.Remark;
            entity.Domain = domain;
            entity.IsEnabled = req.IsEnabled;

            await _tenantRepository.UpdateAsync(entity);
            await _cache.KeyDeleteAsync(SystemCacheKey.AllTenant);
            await _cache.KeyDeleteAsync(SystemCacheKey.TenantDomains);

            if (!req.IsEnabled)
            {
                await this.DisabledTenantSubUserAsync(req.TenantId);
            }
        }

        [Transactional]
        public async Task AssignTenantMenuAsync(AssignTenantMenuRequest req)
        {
            await _tenantMenuRepository.DeleteAsync(x => x.TenantId == req.TenantId);
            if (req.MenuIds?.Length > 0)
            {
                // 找到本次移除的菜单ID，移除租户下所有角色已分配的对应菜单ID
                var existMenuIds = await _roleMenuRepository.Where(x => x.TenantId == req.TenantId).Select(x => x.MenuId).ToListAsync();
                var removeMenuIds = existMenuIds.Where(x => !req.MenuIds.Contains(x)).ToList();
                await _roleMenuRepository.DeleteAsync(x => removeMenuIds.Contains(x.MenuId));

                var tenantMenus = req.MenuIds.Select(id => new TenantMenu
                {
                    TenantId = req.TenantId,
                    MenuId = id
                }).ToList();
                await _tenantMenuRepository.InsertManyAsync(tenantMenus);
            }
            await _identitySharedService.DelUserPermissionCacheByTenantIdAsync(req.TenantId);
        }

        public Task<List<long>> GetTenantMenuIdsAsync(string id)
        {
            return _tenantMenuRepository.Where(x => x.TenantId == id).SelectToListAsync(x => x.MenuId);
        }

        [Transactional]
        public async Task<TenantAccountInfo> CreateTenantAccountAsync(CreateTenantAccountRequest req)
        {
            if (req.ErrCount > 3) throw new BusinessException("创建失败，请联系管理员");
            var info = new TenantAccountInfo()
            {
                RoleName = StringUtils.Generate(12).ToLowerInvariant(),
                UserName = StringUtils.Generate(18).ToLowerInvariant(),
                Password = StringUtils.Generate(18, includeNumbers: true, includeSpecialChars: true, customSpecialChars: "_@"),
            };
            try
            {
                var role = new Role()
                {
                    Id = IdGenerater.Instance.NextId(),
                    TenantId = req.TenantId,
                    RoleName = info.RoleName,
                    Remark = "开通租户账号创建超级管理员（系统创建）",
                    IsEnabled = true
                };
                var user = new User()
                {
                    Id = IdGenerater.Instance.NextId(),
                    TenantId = req.TenantId,
                    UserName = info.UserName,
                    NickName = info.UserName,
                    PasswordSalt = EncryptionUtils.GetPasswordSalt(),
                    Sex = EfCore.Enums.SexType.Male,
                    Avatar = AdminConsts.AvatarMale,
                    IsEnabled = true
                };
                user.Password = EncryptionUtils.GenEncodingPassword(info.Password, user.PasswordSalt);
                var menuIds = await _identitySharedService.GetTenantMenusAsync(req.TenantId);
                var roleMenu = menuIds.Select(x => new RoleMenu() { TenantId = req.TenantId, MenuId = x, RoleId = role.Id }).ToList();
                if (roleMenu.Count > 0) _dbContext.AddRange(roleMenu);

                var userRole = new UserRole { RoleId = role.Id, UserId = user.Id, TenantId = req.TenantId };

                _dbContext.Add(user);
                _dbContext.Add(role);
                _dbContext.Add(userRole);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                req.ErrCount += 3;
                return await this.CreateTenantAccountAsync(req);
            }

            return info;
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

        public async Task<TenantDetails> GetTenantAsync(string id)
        {
            var data = await _tenantRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return new TenantDetails
            {
                TenantId = id,
                Name = data.Name,
                Remark = data.Remark,
                Domain = data.Domain,
                IsEnabled = data.IsEnabled,
                MenuIds = await _tenantMenuRepository.Where(x => x.TenantId == id).SelectToListAsync(x => x.MenuId)
            };
        }
    }
}