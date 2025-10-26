using System.Net;

using Consul;

using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Redis;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Keys;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Monitor
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly IRepository<LoginLog> _loginLogRepository;
        private readonly IHybridCache _hybridCache;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;

        public OnlineUserService(IRepository<LoginLog> loginLogRepository, IHybridCache hybridCache, IRepository<User> userRepository, ICurrentUser currentUser)
        {
            _loginLogRepository = loginLogRepository;
            _hybridCache = hybridCache;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<List<OnlineUserResultDto>> GetOnlineUserListAsync(OnlineUserSearchDto dto)
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
            if (tokenKeys.Count <= 0) return [];

            var result = new List<OnlineUserResultDto>();
            var currentSessionId = _currentUser.FindClaim(AdminConsts.SessionId).Value;
            var subQuery = _loginLogRepository.Where(x => x.CreationTime >= x.CreationTime.AddDays(-1)).GroupBy(x => x.SessionId)
                .Select(g => new
                {
                    SessionId = g.Key,
                    MaxCreationTime = g.Max(x => x.CreationTime)
                });
            var loginLogs = await subQuery
                .Join(_loginLogRepository.GetQueryable(),
                    max => new { max.SessionId, max.MaxCreationTime },
                    log => new { log.SessionId, MaxCreationTime = log.CreationTime },
                    (max, log) => new
                    {
                        UserName = log.UserName,
                        Ip = log.Ip,
                        SessionId = log.SessionId,
                        Address = log.Address,
                        Browser = log.Browser,
                        CreationTime = log.CreationTime
                    })
                .ToListAsync();
            foreach (var key in tokenKeys)
            {
                var arr = key.Split(':');
                if (arr.Length < 3) continue;
                var userId = arr[1];
                var sessionId = arr[2];
                var loginLog = loginLogs.FirstOrDefault(x => x.SessionId == sessionId);
                if (loginLog == null) continue;
                result.Add(new OnlineUserResultDto
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
            if (!string.IsNullOrEmpty(currentSessionId))
            {
                var index = result.FindIndex(x => x.SessionId == currentSessionId);
                if (index > 0)
                {
                    result.Insert(0, result[index]);
                    result.RemoveAt(index + 1);
                }
            }

            return result;
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