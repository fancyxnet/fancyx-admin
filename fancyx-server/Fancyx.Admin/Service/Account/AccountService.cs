using AutoMapper;
using DotNetCore.CAP;
using Fancyx.Admin.IService.Account;
using Fancyx.Admin.IService.Account.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Core.Helpers;
using Fancyx.Core.Interfaces;
using Fancyx.Core.Utils;
using Fancyx.Redis;
using Fancyx.Repository;
using Fancyx.Repository.Entities.Log;
using Fancyx.Repository.Entities.Organization;
using Fancyx.Repository.Entities.System;
using Fancyx.Repository.Enums;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Keys;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fancyx.Admin.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<UserDO> _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<MenuDO> _menuRepository;
        private readonly IConfiguration _configuration;
        private readonly IHybridCache _hybridCache;
        private readonly IdentitySharedService _identitySharedService;
        private readonly ICapPublisher _capPublisher;
        private readonly IMapper _mapper;
        private readonly IRepository<EmployeeDO> _employeeRepository;
        private readonly HttpContext _httpContext;

        public AccountService(IRepository<UserDO> userRepository, ICurrentUser currentUser, IRepository<MenuDO> menuRepository
            , IConfiguration configuration, IHybridCache hybridCache, IdentitySharedService identitySharedService
            , ICapPublisher capPublisher, IHttpContextAccessor httpContextAccessor, IMapper mapper, IRepository<EmployeeDO> employeeRepository)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _menuRepository = menuRepository;
            _configuration = configuration;
            _hybridCache = hybridCache;
            _identitySharedService = identitySharedService;
            _capPublisher = capPublisher;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<(TokenResultDto tokenRes, string sessionId)> GenerateTokenAsync(Guid userId, string userName, string? sessionId = null)
        {
            var time = DateTime.Now;
            var refreshToken = Guid.NewGuid().ToString("N").ToLower();
            sessionId ??= SnowflakeHelper.Instance.NextId().ToString();
            var claims = new List<Claim> {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName),
                new(AdminConsts.SessionId, sessionId)
            };
            //查询用户员工、部门、职位信息
            var employee = await _employeeRepository.GetAsync(x => x.UserId == userId);
            if (employee != null)
            {
                claims.Add(new Claim(AdminConsts.EmployeeId, employee.Id.ToString()));
                if (employee.DeptId.HasValue)
                {
                    claims.Add(new Claim(AdminConsts.DeptId, employee.DeptId.ToString()!));
                }
                if (employee.PositionId.HasValue)
                {
                    claims.Add(new Claim(AdminConsts.PositionId, employee.PositionId.ToString()!));
                }
            }

            var tokenExpired = time.AddHours(AdminConsts.TokenExpiredHour);
            var rs = new LoginResultDto
            {
                UserName = userName,
                ExpiredTime = tokenExpired,
                AccessToken = _identitySharedService.GenerateToken(claims, tokenExpired),
                RefreshToken = refreshToken
            };

            if (!bool.Parse(_configuration["App:AccountManyLogin"]!))
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

        public async Task<TokenResultDto> GetAccessTokenAsync(string refreshToken)
        {
            var sessionId = _currentUser.FindClaim(AdminConsts.SessionId)!.Value;
            var key = SystemCacheKey.RefreshToken(_currentUser.Id!.Value, sessionId);
            if (!await _hybridCache.ExistsAsync(key)) throw new BusinessException(message: "刷新token已过期");

            var existRefreshToken = await _hybridCache.GetAsync<string>(key);
            if (!refreshToken.Equals(existRefreshToken)) throw new BusinessException(message: "刷新token不正确");

            return (await GenerateTokenAsync(_currentUser.Id!.Value, _currentUser.UserName!, sessionId)).tokenRes;
        }

        public async Task<UserAuthInfoDto> GetUserAuthInfoAsync()
        {
            var uid = _currentUser.Id!.Value;
            var user = await _userRepository.GetAsync(x => x.Id == uid) ?? throw new BusinessException(message: "用户不存在");
            var permission = await _identitySharedService.GetUserPermissionAsync(uid);
            UserAuthInfoDto result = new()
            {
                User = new UserInfoDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    Avatar = user.Avatar,
                    Sex = (int)user.Sex,
                    Phone = user.Phone,
                    EmployeeId = await _employeeRepository.Where(x => x.UserId == uid).ToOneAsync(x => x.Id)
                },
                Menus = await GetFrontMenus(),
                Permissions = permission.Auths ?? []
            };
            return result;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto dto)
        {
            var loginLog = new LoginLogDO
            {
                IsSuccess = true,
                Ip = RequestUtils.GetIp(_httpContext),
                OperationMsg = "登录成功",
                UserName = dto.UserName
            };
            try
            {
                var user = await _userRepository.GetAsync(x => x.UserName.ToLower() == dto.UserName.ToLower() && x.IsEnabled) ?? throw new BusinessException(message: "账号或密码不存在");
                var isRight = user.Password == EncryptionUtils.GenEncodingPassword(dto.Password, user.PasswordSalt);
                if (!isRight) throw new BusinessException(message: "密码错误");

                var (tokenRes, sessionId) = await GenerateTokenAsync(user.Id, user.UserName);

                loginLog.SessionId = sessionId;

                var rs = _mapper.Map<TokenResultDto, LoginResultDto>(tokenRes);
                var permission = await _identitySharedService.GetUserPermissionAsync(user.Id);
                rs.UserId = user.Id;
                rs.UserName = dto.UserName;
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
                loginLog.Address = RequestUtils.ResolveAddress(loginLog.Ip);
                loginLog.Browser = RequestUtils.ResolveBrowser(RequestUtils.GetUserAgent(_httpContext));
                await _capPublisher.PublishAsync(AdminEventBusTopicConsts.LoginLogEvent, loginLog);
            }
        }

        public async Task<LoginResultDto> SmsLoginAsync(SmsLoginDto dto)
        {
            var loginLog = new LoginLogDO
            {
                IsSuccess = true,
                Ip = RequestUtils.GetIp(_httpContext),
                OperationMsg = "登录成功",
                UserName = dto.Phone
            };
            try
            {
                var user = await _userRepository.GetAsync(x => x.Phone == dto.Phone && x.IsEnabled) ?? throw new BusinessException(message: "手机号不存在");
                var codeKey = SystemCacheKey.LoginSmsCode(dto.Phone);
                var code = await _hybridCache.GetAsync<string>(codeKey);
                if (string.IsNullOrEmpty(code)) throw new BusinessException("验证码已过期");
                if (dto.Code != code) throw new BusinessException("验证码错误");

                var (tokenRes, sessionId) = await GenerateTokenAsync(user.Id, user.UserName);

                loginLog.SessionId = sessionId;

                var rs = _mapper.Map<TokenResultDto, LoginResultDto>(tokenRes);
                var permission = await _identitySharedService.GetUserPermissionAsync(user.Id);
                rs.UserId = user.Id;
                rs.UserName = dto.Phone;
                rs.SessionId = sessionId;

                await _hybridCache.RemoveAsync(codeKey);

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
                loginLog.Address = RequestUtils.ResolveAddress(loginLog.Ip);
                loginLog.Browser = RequestUtils.ResolveBrowser(RequestUtils.GetUserAgent(_httpContext));
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
            var top = all.Where(x => !x.ParentId.HasValue || x.ParentId == Guid.Empty).OrderBy(x => x.Sort).ToList();
            var topMap = new List<FrontendMenu>();
            foreach (var item in top)
            {
                var mapItem = AutoMapperHelper.Instance.Map<MenuDO, FrontendMenu>(item);
                mapItem.LayerName = item.Title;
                mapItem.Children = getChildren(mapItem);
                topMap.Add(mapItem);
            }

            List<FrontendMenu>? getChildren(FrontendMenu current)
            {
                var children = all.Where(x => x.ParentId == current.Id).OrderBy(x => x.Sort).ToList();
                if (children.Count <= 0) return null;

                var childrenMap = new List<FrontendMenu>();
                foreach (var item in children)
                {
                    var mapItem = AutoMapperHelper.Instance.Map<MenuDO, FrontendMenu>(item);
                    mapItem.LayerName = current.LayerName + "/" + item.Title;
                    mapItem.Children = getChildren(mapItem);
                    childrenMap.Add(mapItem);
                }
                return childrenMap;
            }
            return topMap;
        }

        public async Task<bool> UpdateUserInfoAsync(PersonalInfoDto dto)
        {
            var user = await _userRepository.Where(x => x.Id == _currentUser.Id).FirstAsync();
            if (!string.IsNullOrEmpty(dto.NickName))
            {
                if (user.NickName.ToLower() != dto.NickName!.ToLower())
                {
                    var exist = await _userRepository.Where(x => x.NickName.ToLower() == dto.NickName.ToLower()).AnyAsync();
                    if (exist) throw new BusinessException(message: "昵称已占用");
                }
                user.NickName = dto.NickName;
            }
            if (!string.IsNullOrEmpty(dto.Avatar))
            {
                user.Avatar = dto.Avatar;
            }
            if (dto.Sex > 0)
            {
                user.Sex = dto.Sex;
            }
            if (!string.IsNullOrEmpty(dto.Phone))
            {
                user.Phone = dto.Phone;
            }
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UpdateUserPwdAsync(UserPwdDto dto)
        {
            var user = await _userRepository.Where(x => x.Id == _currentUser.Id).FirstAsync()
                ?? throw new BusinessException(message: "用户不存在");
            var isRight = user.Password == EncryptionUtils.GenEncodingPassword(dto.OldPwd, user.PasswordSalt);
            if (!isRight) throw new BusinessException(message: "旧密码错误");
            user.PasswordSalt = EncryptionUtils.GetPasswordSalt();
            user.Password = EncryptionUtils.GenEncodingPassword(dto.NewPwd, user.PasswordSalt);
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

            var code = StringUtils.RandomStr(6, true);
            await _hybridCache.SetAsync(SystemCacheKey.LoginSmsCode(phone), code, TimeSpan.FromMinutes(5));
            return code;
        }
    }
}