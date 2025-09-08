using Fancyx.Core.Authorization;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Organization;
using Fancyx.DataAccess.Entities.System;
using Fancyx.DataAccess.Enums;
using Fancyx.Redis;
using Fancyx.Shared.Consts;
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
        private readonly IRepository<UserRoleDO> _userRoleRepository;
        private readonly IRepository<RoleMenuDO> _roleMenuRepository;
        private readonly IRepository<RoleDO> _roleRepository;
        private readonly IRepository<MenuDO> _menuRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<UserDO> _userRepository;
        private readonly IHybridCache _hybridCache;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<RoleDeptDO> _roleDeptRepository;
        private readonly IRepository<EmployeeDO> _employeeRepository;
        private readonly IRepository<DeptDO> _deptRepository;

        public IdentitySharedService(IRepository<UserRoleDO> userRoleRepository, IRepository<RoleMenuDO> roleMenuRepository, IRepository<RoleDO> roleRepository,
            IRepository<MenuDO> menuRepository, IConfiguration configuration, IRepository<UserDO> userRepository, IHybridCache hybridCache, ICurrentUser currentUser
            , IRepository<RoleDeptDO> roleDeptRepository, IRepository<EmployeeDO> employeeRepository, IRepository<DeptDO> deptRepository)
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
            _employeeRepository = employeeRepository;
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
            var isSuperAdmin = roles.Any(r => r.RoleName == AdminConsts.SuperAdminRole);
            var menuIds = await _roleMenuRepository.Where(x => roleIds.Contains(x.RoleId)).SelectToListAsync(x => x.MenuId);
            var menus = await _menuRepository.Where(x => menuIds.Contains(x.Id) || isSuperAdmin).SelectToListAsync(x => new { x.Permission, x.Id, x.MenuType });
            if (isSuperAdmin)
            {
                menuIds = menus.Select(x => x.Id).ToList();
            }
            var rs = new UserPermission
            {
                UserId = userId,
                Roles = roles.Select(c => c.RoleName).ToArray(),
                Auths = menus.Where(c => !string.IsNullOrEmpty(c.Permission) && c.MenuType == MenuType.Button).Select(c => c.Permission!).Distinct().ToArray(),
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
        /// 检查Token是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sessionId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CheckTokenAsync(string userId, string sessionId, string token)
        {
            string key = SystemCacheKey.AccessToken(userId, sessionId);
            var existToken = await _hybridCache.GetAsync<string>(key);
            return existToken == token;
        }

        /// <summary>
        /// 检查用户是否有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckPermissionAsync(string userId, string code)
        {
            var permission = await GetUserPermissionAsync(Guid.Parse(userId));
            if (permission == null || permission.Auths == null) return false;

            return permission.Auths.Contains(code) || permission.IsSuperAdmin;
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
            TenantManager.SetCurrent("");
            return _userRepository.AnyAsync(x => x.Id.ToString() == id);
        }

        /// <summary>
        /// 获取当前用户部门数据权限
        /// </summary>
        /// <returns></returns>
        private async Task<(List<Guid> deptIds, List<Guid> employeeIds)> GetCurrentUserDeptPowerAsync()
        {
            Guid? employeeId = Guid.TryParse(_currentUser.FindClaim(AdminConsts.EmployeeId).Value, out var id) ? id : null;
            if (!_currentUser.Id.HasValue || !employeeId.HasValue) return ([], []);

            var key = SystemCacheKey.EmployeeDeptPower(employeeId.Value);
            var cacheData = await _hybridCache.GetAsync<DeptPowerData>(key);
            if (cacheData != null) return (cacheData.DeptIds, cacheData.EmployeeIds);

            var userPermission = await this.GetUserPermissionAsync(_currentUser.Id.Value!);
            if (userPermission.RoleIds == null || userPermission.RoleIds.Length == 0) return ([], []);
            var powerTypes = await _roleRepository.Where(x => userPermission.RoleIds.Contains(x.Id)).Distinct().SelectToListAsync(x => x.DeptPowerType);
            if (powerTypes == null || powerTypes.Count == 0) return ([], []);

            var deptIds = new List<Guid>();
            var employeeIds = new List<Guid>();
            Guid? curDeptId = Guid.TryParse(_currentUser.FindClaim(AdminConsts.DeptId).Value, out var _deptId) ? _deptId : null;
            foreach (var powerType in powerTypes)
            {
                if (powerType == DeptPowerType.All)
                {
                    //所有部门
                    deptIds.AddRange(await _deptRepository.GetQueryable().SelectToListAsync(x => x.Id));
                    //所有员工
                    employeeIds.AddRange(await _employeeRepository.GetQueryable().SelectToListAsync(x => x.Id));
                    break;
                }
                switch (powerType)
                {
                    case DeptPowerType.ThisLevel:
                        if (curDeptId.HasValue)
                        {
                            deptIds.Add(curDeptId.Value);
                        }
                        break;

                    case DeptPowerType.ThisLevelAndBelow:
                        if (curDeptId.HasValue)
                        {
                            deptIds.Add(curDeptId.Value);
                            //以下部门
                            var subDept = await _deptRepository.Where(x => x.ParentId == curDeptId).SelectToListAsync(x => x.Id);
                            deptIds.AddRange(subDept);
                        }
                        break;

                    case DeptPowerType.Specify:
                        var specifyDeptIds = await _roleDeptRepository.Where(x => userPermission.RoleIds.Contains(x.RoleId)).SelectToListAsync(x => x.DeptId);
                        deptIds.AddRange(specifyDeptIds);
                        break;

                    case DeptPowerType.OnlyMe:
                        employeeIds.Add(employeeId.Value);
                        break;
                }
            }
            if (deptIds.Count > 0)
            {
                var findEmployeeIds = await _employeeRepository.Where(x => x.DeptId != null && deptIds.Contains(x.DeptId.Value)).SelectToListAsync(x => x.Id);
                employeeIds.AddRange(findEmployeeIds);
            }
            deptIds = deptIds.Distinct().ToList();
            employeeIds = employeeIds.Distinct().ToList();

            await _hybridCache.SetAsync(key, new DeptPowerData { DeptIds = deptIds, EmployeeIds = employeeIds });
            return (deptIds, employeeIds);
        }

        /// <summary>
        /// 清除当前用户部门数据权限
        /// </summary>
        /// <returns></returns>
        public async Task ClearCurrentUserDeptPower()
        {
            Guid? employeeId = Guid.TryParse(_currentUser.FindClaim(AdminConsts.EmployeeId).Value, out var id) ? id : null;
            if (!_currentUser.Id.HasValue || !employeeId.HasValue) return;

            var key = SystemCacheKey.EmployeeDeptPower(employeeId.Value);
            await _hybridCache.RemoveAsync(key);
        }
    }
}