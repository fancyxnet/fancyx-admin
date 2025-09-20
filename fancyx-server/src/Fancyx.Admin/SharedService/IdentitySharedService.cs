using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Redis;
using Fancyx.Shared.EfCore;
using Fancyx.Shared.Keys;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fancyx.Admin.SharedService
{
    public class IdentitySharedService : IScopedDependency
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleMenu> _roleMenuRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;
        private readonly IHybridCache _hybridCache;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<RoleDept> _roleDeptRepository;
        private readonly IRepository<Dept> _deptRepository;

        public IdentitySharedService(IRepository<UserRole> userRoleRepository, IRepository<RoleMenu> roleMenuRepository, IRepository<Role> roleRepository,
            IRepository<Menu> menuRepository, IConfiguration configuration, IRepository<User> userRepository, IHybridCache hybridCache, ICurrentUser currentUser
            , IRepository<RoleDept> roleDeptRepository, IRepository<Dept> deptRepository)
        {
            _userRoleRepository = userRoleRepository;
            _roleMenuRepository = roleMenuRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _hybridCache = hybridCache;
            _currentUser = currentUser;
            _roleDeptRepository = roleDeptRepository;
            _deptRepository = deptRepository;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserPermission> GetUserPermissionAsync(Guid userId)
        {
            var key = SystemCacheKey.UserPermission(userId);
            if (await _hybridCache.ExistsAsync(key))
            {
                var cacheValue = await _hybridCache.GetAsync<UserPermission>(key);
                return cacheValue!;
            }

            var roleIds = await _userRoleRepository.Where(x => x.UserId == userId).SelectToListAsync(x => x.RoleId);
            var roles = await _roleRepository.Where(x => roleIds.Contains(x.Id) && x.IsEnabled).ToListAsync();
            var isSuperAdmin = roles.Any(r => r.RoleName == DataPower.SuperAdmin);
            var menuIds = await _roleMenuRepository.Where(x => roleIds.Contains(x.RoleId)).SelectToListAsync(x => x.MenuId);
            var menus = await _menuRepository.Where(x => menuIds.Contains(x.Id) || isSuperAdmin).SelectToListAsync(x => new { x.Permission, x.Id, x.MenuType, x.Display });
            if (isSuperAdmin)
            {
                menuIds = menus.Select(x => x.Id).ToList();
            }
            var rs = new UserPermission
            {
                UserId = userId,
                Roles = roles.Select(c => c.RoleName).ToArray(),
                Auths = menus.Where(c => !string.IsNullOrEmpty(c.Permission) && c.MenuType == MenuType.Button && c.Display).Select(c => c.Permission!).Distinct().ToArray(),
                RoleIds = [.. roleIds],
                MenuIds = [.. menuIds],
                IsSuperAdmin = isSuperAdmin
            };
            await _hybridCache.SetAsync(key, rs);
            return rs;
        }

        /// <summary>
        /// 删除用户权限缓存（通过角色ID）
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DelUserPermissionCacheByRoleIdAsync(Guid roleId)
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
        public Task DelUserPermissionCacheByUserIdAsync(Guid userId)
        {
            return _hybridCache.RemoveAsync(SystemCacheKey.UserPermission(userId));
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
        /// 用户是否来源主库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> UserIsFromMainDbAsync(string id)
        {
            return _userRepository.GetQueryable().AsNoTracking().IgnoreQueryFilters().AnyAsync(x => !x.IsDeleted && x.Id.ToString() == id);
        }

        /// <summary>
        /// 初始化用户部门数据权限
        /// </summary>
        /// <returns></returns>
        public async Task<DeptPowerData?> GetUserDeptPowerAsync(Guid userId, Guid? curDeptId)
        {
            var key = SystemCacheKey.UserDeptPower(userId);
            if (await _hybridCache.ExistsAsync(key)) return await _hybridCache.GetAsync<DeptPowerData>(key);

            var userPermission = await this.GetUserPermissionAsync(userId);
            if (userPermission.RoleIds == null || userPermission.RoleIds.Length == 0) return null;
            var powerTypes = await _roleRepository.Where(x => userPermission.RoleIds.Contains(x.Id)).Distinct().SelectToListAsync(x => x.DeptPowerType);
            if (powerTypes == null || powerTypes.Count == 0) return null;

            var deptIds = new List<Guid>();
            var userIds = new List<Guid>();
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
            await _hybridCache.SetAsync(key, result);
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
            await _hybridCache.RemoveAsync(key);
        }
    }
}