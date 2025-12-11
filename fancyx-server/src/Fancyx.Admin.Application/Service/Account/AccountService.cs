using AutoMapper;

using DotNetCore.CAP;
using Fancyx.Admin.Application.IService.Account;
using Fancyx.Admin.Application.IService.Account.Models;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Core;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Redis;
using Fancyx.Shared.Consts;
using Fancyx.Shared.EfCore;
using Fancyx.Shared.Keys;
using Fancyx.SnowflakeId;
using Fancyx.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Fancyx.Admin.Application.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IConfiguration _configuration;
        private readonly IHybridCache _hybridCache;
        private readonly IdentitySharedService _identitySharedService;
        private readonly ICapPublisher _capPublisher;
        private readonly IMapper _mapper;
        private readonly ICurrentTenant _currentTenant;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly HttpContext _httpContext;

        private delegate Task<User> LoginHandler();

        public AccountService(IRepository<User> userRepository, ICurrentUser currentUser, IRepository<Menu> menuRepository
            , IConfiguration configuration, IHybridCache hybridCache, IdentitySharedService identitySharedService
            , ICapPublisher capPublisher, IHttpContextAccessor httpContextAccessor, IMapper mapper, ICurrentTenant currentTenant
            , IRepository<Tenant> tenantRepository)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _menuRepository = menuRepository;
            _configuration = configuration;
            _hybridCache = hybridCache;
            _identitySharedService = identitySharedService;
            _capPublisher = capPublisher;
            _mapper = mapper;
            _currentTenant = currentTenant;
            _tenantRepository = tenantRepository;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="otherClaims">其它证件单元</param>
        /// <returns></returns>
        private async Task<(TokenResponse tokenRes, string sessionId)> GenerateTokenAsync(long userId, string userName, string? sessionId = null, IEnumerable<Claim>? otherClaims = null)
        {
            var time = DateTime.Now;
            var refreshToken = Guid.NewGuid().ToString("N").ToLower();
            sessionId ??= IdGenerater.Instance.NextId().ToString();
            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName),
                new(AdminConsts.SessionId, sessionId)
            };
            if (otherClaims != null)
            {
                foreach (var claim in otherClaims)
                {
                    if (claims.Any(x => x.Type == claim.Type)) continue;
                    claims.Add(claim);
                }
            }

            var tokenExpired = time.AddHours(AdminConsts.TokenExpiredHour);
            var rs = new LoginRespone
            {
                UserName = userName,
                ExpiredTime = tokenExpired,
                AccessToken = _identitySharedService.GenerateToken(claims, tokenExpired),
                RefreshToken = refreshToken
            };

            if (!bool.Parse(_configuration["AccountManyLogin"]!))
            {
                //移除其它记录token
                await _hybridCache.RemoveByPatternAsync(SystemCacheKey.AccessToken(userId, "*"));
                await _hybridCache.RemoveByPatternAsync(SystemCacheKey.RefreshToken(userId, "*"));
            }

            var expired = TimeSpan.FromHours(AdminConsts.TokenExpiredHour);
            await _hybridCache.SetAsync(SystemCacheKey.AccessToken(userId, sessionId), rs.AccessToken, expired);
            await _hybridCache.SetAsync(SystemCacheKey.RefreshToken(userId, sessionId), refreshToken, TimeSpan.FromMinutes(AdminConsts.TokenExpiredHour * 60 + 20));

            return (rs, sessionId);
        }

        public async Task<TokenResponse> GetAccessTokenAsync(string refreshToken)
        {
            var sessionId = _currentUser.FindClaim(AdminConsts.SessionId)!.Value;
            var key = SystemCacheKey.RefreshToken(_currentUser.Id!.Value, sessionId);
            if (!await _hybridCache.ExistsAsync(key)) throw new BusinessException(message: "刷新token已过期");

            var existRefreshToken = await _hybridCache.GetAsync<string>(key);
            if (!refreshToken.Equals(existRefreshToken)) throw new BusinessException(message: "刷新token不正确");

            var otherClaims = _currentUser.GetClaims();
            return (await GenerateTokenAsync(_currentUser.Id!.Value, _currentUser.UserName!, sessionId, otherClaims)).tokenRes;
        }

        public async Task<GetUserAuthInfoResponse> GetUserAuthInfoAsync()
        {
            var uid = _currentUser.Id!.Value;
            var user = await _userRepository.FindAsync(uid) ?? throw new BusinessException(message: "用户不存在");
            var permission = await _identitySharedService.GetUserPermissionAsync(uid);
            GetUserAuthInfoResponse result = new()
            {
                User = new CurrentUserInfo
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    Avatar = user.Avatar,
                    Sex = (int)user.Sex,
                    Phone = user.Phone
                },
                Menus = await GetFrontMenus(),
                Permissions = permission.Auths ?? []
            };
            return result;
        }

        public async Task<LoginRespone> LoginAsync(LoginRequest req)
        {
            return await InternalLoginAsync(req.UserName, async () =>
            {
                var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == req.UserName.ToLower() && x.IsEnabled) ?? throw new BusinessException(message: "账号或密码不存在");
                var isOk = user.Password == EncryptionUtils.GenEncodingPassword(req.Password, user.PasswordSalt);
                if (!isOk) throw new BusinessException(message: "密码错误");

                await this.CheckTenantIsEnabledAsync(user.TenantId);

                return user;
            });
        }

        public async Task<LoginRespone> SmsLoginAsync(SmsLoginRequest req)
        {
            return await InternalLoginAsync(req.Phone, async () =>
            {
                var user = await _userRepository.GetAsync(x => x.Phone == req.Phone && x.IsEnabled) ?? throw new BusinessException(message: "手机号不存在");
                var codeKey = SystemCacheKey.LoginSmsCode(req.Phone);
                var code = await _hybridCache.GetAsync<string>(codeKey);
                if (string.IsNullOrEmpty(code)) throw new BusinessException("验证码已过期");
                if (req.Code != code) throw new BusinessException("验证码错误");

                await this.CheckTenantIsEnabledAsync(user.TenantId);

                await _hybridCache.RemoveAsync(codeKey);
                return user;
            });
        }

        private async Task CheckTenantIsEnabledAsync(string? tenantId)
        {
            var tenantIsEnabled = await _tenantRepository.AnyAsync(x => x.Id == tenantId && x.IsEnabled);
            if (MultiTenancyConsts.IsEnabled && !tenantIsEnabled)
            {
                throw new BusinessException("租户已禁用");
            }
        }

        private async Task<LoginRespone> InternalLoginAsync(string userName, LoginHandler loginHandler)
        {
            var loginLog = new LoginLog
            {
                IsSuccess = true,
                Ip = HttpUtils.GetIp(_httpContext),
                OperationMsg = "登录成功",
                UserName = userName,
                TenantId = _currentTenant.TenantId
            };
            try
            {
                var user = await loginHandler();
                var claims = new List<Claim>();

                // 组织信息
                if (user.DeptId.HasValue) claims.Add(new Claim(DataPower.DeptId, user.DeptId.Value.ToString()));
                if (user.PostId.HasValue) claims.Add(new Claim(AdminConsts.PostId, user.PostId.Value.ToString()));

                // 权限：可查看部门和用户
                var powerData = await _identitySharedService.GetUserDeptPowerAsync(user.Id, user.DeptId);
                if (powerData != null)
                {
                    if (powerData.DeptIds?.Count > 0)
                    {
                        claims.Add(new Claim(DataPower.DeptIdType, string.Join(',', powerData.DeptIds)));
                    }
                    if (powerData.UserIds?.Count > 0)
                    {
                        claims.Add(new Claim(DataPower.UserIdType, string.Join(',', powerData.UserIds)));
                    }
                }

                var permission = await _identitySharedService.GetUserPermissionAsync(user.Id);
                if (permission.Roles?.Length > 0) claims.Add(new Claim(ClaimTypes.Role, string.Join(',', permission.Roles)));

                var (tokenRes, sessionId) = await GenerateTokenAsync(user.Id, user.UserName, otherClaims: claims);
                loginLog.SessionId = sessionId;
                var rs = _mapper.Map<TokenResponse, LoginRespone>(tokenRes);
                rs.UserId = user.Id;
                rs.UserName = user.UserName;
                rs.SessionId = sessionId;

                return rs;
            }
            catch (BusinessException ex)
            {
                loginLog.IsSuccess = false;
                loginLog.OperationMsg = ex.Message;
                throw;
            }
            finally
            {
                loginLog.Address = IpAddressUtils.ResolveAddress(loginLog.Ip);
                loginLog.Browser = HttpUtils.GetBrowserByUA(_httpContext.Request.Headers.UserAgent);
                await _capPublisher.PublishAsync(AdminEventBusTopicConsts.LoginLogEvent, loginLog);
            }
        }

        public async Task<List<FrontendMenu>> GetFrontMenus()
        {
            var uid = _currentUser.Id!.Value;
            var permission = await _identitySharedService.GetUserPermissionAsync(uid);
            if (permission.MenuIds == null || permission.MenuIds.Length <= 0) return [];

            var all = await _menuRepository
                .Where(x => permission.MenuIds.Contains(x.Id) && (x.MenuType == MenuType.Menu || x.MenuType == MenuType.Folder)).ToListAsync();
            var roots = CollectionUtils.BuildTree(all, g => new FrontendMenu
            {
                Id = g.Id,
                Title = g.Title,
                Icon = g.Icon,
                Path = g.Path,
                MenuType = (int)g.MenuType,
                Display = g.Display,
                Component = g.Component,
                IsExternal = g.IsExternal,
                KeepAlive = g.KeepAlive,
                Sort = g.Sort
            }, g => g.Id, g => g.ParentId, (node, children) => node.Children = children, node => node.Sort);

            CollectionUtils.SetLayerNames(
                roots: roots,
                getTitle: n => n.Title ?? "",
                setLayerName: (n, p) => n.LayerName = p,
                getChildren: n => n.Children,
                separator: "/"
            );

            return roots;

            //var top = all.Where(x => !x.ParentId.HasValue).OrderBy(x => x.Sort).ToList();
            //var topMap = new List<FrontendMenu>();
            //foreach (var item in top)
            //{
            //    var mapItem = _mapper.Map<Menu, FrontendMenu>(item);
            //    mapItem.LayerName = item.Title;
            //    mapItem.Children = getChildren(mapItem);
            //    topMap.Add(mapItem);
            //}

            //List<FrontendMenu>? getChildren(FrontendMenu current)
            //{
            //    var children = all.Where(x => x.ParentId == current.Id).OrderBy(x => x.Sort).ToList();
            //    if (children.Count <= 0) return null;

            //    var childrenMap = new List<FrontendMenu>();
            //    foreach (var item in children)
            //    {
            //        var mapItem = _mapper.Map<Menu, FrontendMenu>(item);
            //        mapItem.LayerName = current.LayerName + "/" + item.Title;
            //        mapItem.Children = getChildren(mapItem);
            //        childrenMap.Add(mapItem);
            //    }
            //    return childrenMap;
            //}
            //return topMap;
        }

        public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoRequest req)
        {
            var user = await _userRepository.Where(x => x.Id == _currentUser.Id).FirstAsync();
            if (!string.IsNullOrEmpty(req.NickName))
            {
                if (user.NickName.ToLower() != req.NickName!.ToLower())
                {
                    var exist = await _userRepository.Where(x => x.NickName.ToLower() == req.NickName.ToLower()).AnyAsync();
                    if (exist) throw new BusinessException(message: "昵称已占用");
                }
                user.NickName = req.NickName;
            }
            if (!string.IsNullOrEmpty(req.Avatar))
            {
                user.Avatar = req.Avatar;
            }
            if (req.Sex > 0)
            {
                user.Sex = req.Sex;
            }
            if (!string.IsNullOrEmpty(req.Phone))
            {
                user.Phone = req.Phone;
            }
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UpdateUserPwdAsync(UpdateUserPwdRequest req)
        {
            var user = await _userRepository.Where(x => x.Id == _currentUser.Id).FirstAsync()
                ?? throw new BusinessException(message: "用户不存在");
            var isCorrect = user.Password == EncryptionUtils.GenEncodingPassword(req.OldPwd, user.PasswordSalt);
            if (!isCorrect) throw new BusinessException(message: "旧密码错误");
            user.PasswordSalt = EncryptionUtils.GetPasswordSalt();
            user.Password = EncryptionUtils.GenEncodingPassword(req.NewPwd, user.PasswordSalt);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> SignOutAsync()
        {
            var uid = _currentUser.Id;
            if (!uid.HasValue) return false;
            var sessionId = _currentUser.FindClaim(AdminConsts.SessionId)!.Value;
            //移除访问token
            await _hybridCache.RemoveAsync(SystemCacheKey.AccessToken(uid.Value, sessionId));
            //移除刷新token
            await _hybridCache.RemoveAsync(SystemCacheKey.RefreshToken(uid.Value, sessionId));
            //移除权限缓存
            await _hybridCache.RemoveAsync(SystemCacheKey.UserPermission(uid.Value));
            await _identitySharedService.ClearCurrentUserDeptPower();
            return true;
        }

        public async Task<string> SendLoginSmsCodeAsync(string phone)
        {
            var userIsExist = await _userRepository.Where(x => x.Phone == phone).AnyAsync();
            if (!userIsExist)
            {
                throw new BusinessException("手机号不存在");
            }

            var code = StringUtils.RandomCode(6);
            await _hybridCache.SetAsync(SystemCacheKey.LoginSmsCode(phone), code, TimeSpan.FromMinutes(5));
            return code;
        }
    }
}