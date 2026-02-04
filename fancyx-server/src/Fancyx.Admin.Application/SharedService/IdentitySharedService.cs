using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Cracker.Caching;
using Cracker.AspNetCore.AutoInject;
using Cracker.EfCore;
using Fancyx.Shared.Keys;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Role = Fancyx.Admin.EfCore.Entities.System.Role;
using Cracker.IdentityServer.Abstractions;
using Fancyx.Shared;

namespace Fancyx.Admin.Application.SharedService
{
    [DependencyInject(AsSelf = true)]
    public class IdentitySharedService
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleMenu> _roleMenuRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;
        private readonly ICacheClient _cache;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<RoleDept> _roleDeptRepository;
        private readonly IRepository<Dept> _deptRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<TenantMenu> _tenantMenuRepository;
        private readonly ICurrentTenant _currentTenant;

        public IdentitySharedService(IRepository<UserRole> userRoleRepository, IRepository<RoleMenu> roleMenuRepository, IRepository<Role> roleRepository,
            IRepository<Menu> menuRepository, IConfiguration configuration, IRepository<User> userRepository, ICacheClient cache, ICurrentUser currentUser
            , IRepository<RoleDept> roleDeptRepository, IRepository<Dept> deptRepository, IRepository<Tenant> tenantRepository, IRepository<TenantMenu> tenantMenuRepository
            , ICurrentTenant currentTenant)
        {
            _userRoleRepository = userRoleRepository;
            _roleMenuRepository = roleMenuRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _cache = cache;
            _currentUser = currentUser;
            _roleDeptRepository = roleDeptRepository;
            _deptRepository = deptRepository;
            _tenantRepository = tenantRepository;
            _tenantMenuRepository = tenantMenuRepository;
            _currentTenant = currentTenant;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserPermission> GetUserPermissionAsync(long userId)
        {
            var key = SystemCacheKey.UserPermission(userId);
            if (await _cache.KeyExistsAsync(key))
            {
                var cacheValue = await _cache.GetAsync<UserPermission>(key);
                return cacheValue!;
            }

            var roles = await (from r in _roleRepository.GetQueryable()
                         join ur in _userRoleRepository.GetQueryable() on r.Id equals ur.RoleId
                         where ur.UserId == userId && r.IsEnabled
                         group r by new { r.Id, r.RoleName } into g
                         select new { g.Key.Id, g.Key.RoleName }).ToListAsync();
            var roleIds = roles.Select(x => x.Id).ToList();
            var menus = await (from m in _menuRepository.GetQueryable()
                         join rm in _roleMenuRepository.GetQueryable() on m.Id equals rm.MenuId
                         where roleIds.Contains(rm.RoleId)
                         select new
                         {
                             m.Id,
                             m.Permission,
                             m.MenuType,
                             m.Display,
                         }).ToListAsync();
            if (MultiTenancyVars.IsEnabled)
            {
                var tenantMenuIds = await this.GetTenantMenusAsync(_currentTenant.TenantId!);
                menus = menus.Where(x => tenantMenuIds.Contains(x.Id)).ToList();
            }
            var rs = new UserPermission
            {
                UserId = userId,
                Roles = [.. roles.Select(c => c.RoleName)],
                Auths = menus.Where(c => !string.IsNullOrEmpty(c.Permission) && c.MenuType == MenuType.Button && c.Display).Select(c => c.Permission!).Distinct().ToArray(),
                RoleIds = roleIds,
                MenuIds = [.. menus.Select(x => x.Id)]
            };
            await _cache.SetAsync(key, rs);
            return rs;
        }

        /// <summary>
        /// 删除用户权限缓存（通过角色ID）
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DelUserPermissionCacheByRoleIdAsync(long roleId)
        {
            var userRoles = await _userRoleRepository.Where(x => x.RoleId == roleId).ToListAsync();
            foreach (var item in userRoles)
            {
                await DelUserPermissionCacheByUserIdAsync(item.UserId);
            }
        }

        /// <summary>
        /// 删除用户权限缓存（通过用户ID）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task DelUserPermissionCacheByUserIdAsync(long userId)
        {
            return _cache.KeyDeleteAsync(SystemCacheKey.UserPermission(userId));
        }

        /// <summary>
        /// 删除用户权限缓存（通过租户ID）
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clearToken"></param>
        /// <returns></returns>
        public async Task DelUserPermissionCacheByTenantIdAsync(string tenantId, bool clearToken = false)
        {
            var userIds = await _userRepository.GetQueryable().IgnoreQueryFilters().Where(x => x.TenantId == tenantId).SelectToListAsync(x => x.Id);
            foreach (var userId in userIds)
            {
                await DelUserPermissionCacheByUserIdAsync(userId);
                if (clearToken)
                {
                    await _cache.KeyDeleteByPatternAsync(SystemCacheKey.AccessToken(userId, "*"));
                    await _cache.KeyDeleteByPatternAsync(SystemCacheKey.RefreshToken(userId, "*"));
                }
            }
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public string GenerateToken(IEnumerable<Claim> claims, DateTime expireTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt")["IssuerSigningKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt")["ValidIssuer"],
                audience: _configuration.GetSection("Jwt")["ValidAudience"],
                claims: claims,
                expires: expireTime,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        /// <summary>
        /// 从Token中获取用户身份
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal? GetPrincipalFromAccessToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt")["IssuerSigningKey"]!));
                return handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化用户部门数据权限
        /// </summary>
        /// <returns></returns>
        public async Task<DeptPowerData?> GetUserDeptPowerAsync(long userId, long? curDeptId)
        {
            var key = SystemCacheKey.UserDeptPower(userId);
            if (await _cache.KeyExistsAsync(key)) return await _cache.GetAsync<DeptPowerData>(key);

            var userPermission = await GetUserPermissionAsync(userId);
            if (userPermission.RoleIds == null || userPermission.RoleIds.Count == 0) return null;
            var powerTypes = await _roleRepository.Where(x => userPermission.RoleIds.Contains(x.Id)).Distinct().SelectToListAsync(x => x.DeptPowerType);
            if (powerTypes == null || powerTypes.Count == 0) return null;

            var deptIds = new List<long>();
            var userIds = new List<long>();
            var isAll = powerTypes.Any(x => x == DeptPowerType.All);
            if (isAll)
            {
                deptIds = await _deptRepository.GetQueryable().SelectToListAsync(x => x.Id);
                userIds = await _userRepository.GetQueryable().SelectToListAsync(x => x.Id);
            }
            else
            {
                var inThisLevel = false;
                var inThisLevelAndBelow = false;
                foreach (var powerType in powerTypes)
                {
                    if (!inThisLevelAndBelow && powerType == DeptPowerType.ThisLevel)
                    {
                        if (curDeptId.HasValue) deptIds.Add(curDeptId.Value);
                        inThisLevel = true;
                    }
                    else if (powerType == DeptPowerType.ThisLevelAndBelow)
                    {
                        if (curDeptId.HasValue)
                        {
                            var selfAndSubDept = await _deptRepository.Where(x => x.TreePath.Contains(curDeptId.Value.ToString())).SelectToListAsync(x => x.Id);
                            deptIds.AddRange(selfAndSubDept);
                        }
                        inThisLevel = true;
                        inThisLevelAndBelow = true;
                    }
                    else if (powerType == DeptPowerType.Specify)
                    {
                        var specifyDeptIds = await _roleDeptRepository.Where(x => userPermission.RoleIds.Contains(x.RoleId)).SelectToListAsync(x => x.DeptId);
                        deptIds.AddRange(specifyDeptIds);
                    }
                    else if (!(inThisLevel || inThisLevelAndBelow) && powerType == DeptPowerType.OnlyMe)
                    {
                        userIds.Add(userId);
                    }
                }

                if (deptIds.Count > 0)
                {
                    deptIds = [.. deptIds.Distinct()];
                    var findUserIds = await _userRepository.Where(x => x.DeptId != null && deptIds.Contains(x.DeptId.Value)).SelectToListAsync(x => x.Id);
                    userIds.AddRange(findUserIds);
                }
                userIds = [.. userIds.Distinct()];
            }

            var result = new DeptPowerData { DeptIds = deptIds, UserIds = userIds };
            await _cache.SetAsync(key, result);
            return result;
        }

        /// <summary>
        /// 清除当前用户部门数据权限
        /// </summary>
        /// <returns></returns>
        public async Task ClearCurrentUserDeptPower()
        {
            if (!_currentUser.Id.HasValue) return;

            var key = SystemCacheKey.UserDeptPower(_currentUser.Id.Value);
            await _cache.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 通过租户ID查询租户拥有菜单
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Task<List<long>> GetTenantMenusAsync(string tenantId)
        {
            return _tenantMenuRepository.GetQueryable().Where(x => x.TenantId == tenantId).Join(_tenantRepository.GetQueryable(), tm => tm.TenantId, t => t.Id, (tm, t) => tm.MenuId).ToListAsync();
        }
    }
}