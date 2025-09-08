using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.Monitor;
using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Repository;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Keys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using StackExchange.Redis;

namespace Fancyx.Admin.Service.Monitor
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly IRepository<LoginLogDO> _loginLogRepository;
        private readonly IDatabase _redisDb;
        private readonly IRepository<UserDO> _userRepository;
        private readonly IMemoryCache _memoryCache;

        public OnlineUserService(IRepository<LoginLogDO> loginLogRepository, IDatabase redisDb, IRepository<UserDO> userRepository, IMemoryCache memoryCache)
        {
            _loginLogRepository = loginLogRepository;
            _redisDb = redisDb;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<PagedResult<OnlineUserResultDto>> GetOnlineUserListAsync(OnlineUserSearchDto dto)
        {
            //有效token时间，1分钟误差
            var time = DateTime.Now.AddHours(-AdminConsts.TokenExpiredHour).AddMinutes(-1);

            var loginLogs = await _loginLogRepository.Where(x => x.CreationTime >= time)
                .Where(x => x.IsSuccess && !string.IsNullOrEmpty(x.SessionId))
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName.Contains(dto.UserName!))
                .OrderByDescending(x => x.CreationTime).ToListAsync();
            var userNames = loginLogs.Select(x => x.UserName).ToList();
            var users = await _userRepository.Where(x => userNames.Contains(x.UserName)).Select(x => new { x.Id, x.UserName }).ToListAsync();

            var list = new List<OnlineUserResultDto>();
            foreach (var loginLog in loginLogs)
            {
                var user = users.FirstOrDefault(x => x.UserName == loginLog.UserName);
                if (user == null) continue;

                if (!string.IsNullOrEmpty(loginLog.SessionId) && await _redisDb.KeyExistsAsync(SystemCacheKey.AccessToken(user.Id, loginLog.SessionId)))
                {
                    list.Add(new OnlineUserResultDto
                    {
                        UserId = user.Id,
                        UserName = loginLog.UserName,
                        Ip = loginLog.Ip,
                        Address = loginLog.Address,
                        Browser = loginLog.Browser,
                        CreationTime = loginLog.CreationTime,
                        SessionId = loginLog.SessionId
                    });
                }
            }
            var total = list.Count;

            return new PagedResult<OnlineUserResultDto>(dto)
            {
                TotalCount = total,
                Items = list.OrderByDescending(s => s.CreationTime).Skip((dto.Current - 1) * dto.PageSize).Take(dto.PageSize).ToList()
            };
        }

        public async Task LogoutAsync(string key)
        {
            //移除访问token
            await _redisDb.KeyDeleteAsync(SystemCacheKey.AccessToken(key));
            _memoryCache.Remove(SystemCacheKey.AccessToken(key));
            //移除刷新token
            await _redisDb.KeyDeleteAsync(SystemCacheKey.RefreshToken(key));
        }
    }
}