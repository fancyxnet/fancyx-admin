using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.Redis;
using Fancyx.Shared.Keys;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Monitor
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly IRepository<LoginLog> _loginLogRepository;
        private readonly IHybridCache _hybridCache;
        private readonly IRepository<User> _userRepository;

        public OnlineUserService(IRepository<LoginLog> loginLogRepository, IHybridCache hybridCache, IRepository<User> userRepository)
        {
            _loginLogRepository = loginLogRepository;
            _hybridCache = hybridCache;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<OnlineUserResultDto>> GetOnlineUserListAsync(OnlineUserSearchDto dto)
        {
            var pattern = SystemCacheKey.AccessToken("*:*");
            if (!string.IsNullOrEmpty(dto.UserName))
            {
                long? queryUserId = await _userRepository.Where(x => x.UserName == dto.UserName).ToOneAsync(x => x.Id);
                if (queryUserId.HasValue)
                {
                    pattern = SystemCacheKey.AccessToken($"{queryUserId}:*");
                }
            }
            var tokenKeys = await _hybridCache.KeyPatternAsync(pattern);
            var resp = new PagedResult<OnlineUserResultDto>(dto) { TotalCount = tokenKeys.Count };
            if (tokenKeys.Count <= 0) return resp;
            resp.Items = new List<OnlineUserResultDto>();
            var loginLogs = await _loginLogRepository.Where(x => x.CreationTime <= x.CreationTime.AddDays(1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            foreach (var key in tokenKeys)
            {
                if (resp.Items.Count >= dto.PageSize)
                {
                    return resp;
                }
                var arr = key.Split(':');
                if (arr.Length < 3) continue;
                var userId = arr[1];
                var sessionId = arr[2];
                var loginLog = loginLogs.FirstOrDefault(x => x.SessionId == sessionId);
                if (loginLog == null) continue;
                resp.Items.Add(new OnlineUserResultDto
                {
                    UserId = userId,
                    UserName = loginLog.UserName,
                    Ip = loginLog.Ip,
                    Address = loginLog.Address,
                    Browser = loginLog.Browser,
                    CreationTime = loginLog.CreationTime,
                    SessionId = loginLog.SessionId
                });
            }

            return resp;
        }

        public async Task LogoutAsync(string key)
        {
            //移除访问token
            await _hybridCache.RemoveAsync(SystemCacheKey.AccessToken(key));
            //移除刷新token
            await _hybridCache.RemoveAsync(SystemCacheKey.RefreshToken(key));
        }
    }
}