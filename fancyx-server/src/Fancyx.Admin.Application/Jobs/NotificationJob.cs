using Coravel.Invocable;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.Application.WebSockets;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Cache;
using Fancyx.Core.AutoInject;
using Fancyx.EfCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedLockNet.SERedis;

using StackExchange.Redis;
using System.Threading.Channels;

namespace Fancyx.Admin.Application.Jobs
{
    [DependencyInject(AsSelf = true)]
    public class NotificationJob : IInvocable
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly IRepository<Notification> _repository;
        private readonly ICacheClient _cache;
        private readonly RedLockFactory _redLockFactory;
        private readonly ChannelWriter<NotificationMessage> _channelWriter;

        public NotificationJob(ILogger<NotificationJob> logger, IRepository<Notification> repository, ICacheClient cache
            , RedLockFactory redLockFactory, ChannelWriter<NotificationMessage> channelWriter)
        {
            _logger = logger;
            _repository = repository;
            _cache = cache;
            _redLockFactory = redLockFactory;
            _channelWriter = channelWriter;
        }

        public async Task Invoke()
        {
            try
            {
                var expiry = TimeSpan.FromSeconds(30);
                var wait = TimeSpan.FromSeconds(10);
                var retry = TimeSpan.FromSeconds(1);

                using var redLock = await _redLockFactory.CreateLockAsync(nameof(NotificationJob), expiry, wait, retry);
                if (redLock.IsAcquired)
                {
                    var notis = await _repository.GetQueryable().IgnoreQueryFilters().Where(x => !x.IsReaded).ToListAsync();
                    var groupMap = notis.GroupBy(x => x.UserId).ToDictionary(k => k.Key, v => v.Count());
                    var random = new Random();
                    if (notis.Count > 0)
                    {
                        foreach (var g in groupMap)
                        {
                            var curEmployeeNotis = notis.Where(x => x.UserId == g.Key).ToList();
                            var index = random.Next(0, curEmployeeNotis.Count);
                            var item = curEmployeeNotis[index];
                            var lastNotiKey = "LastNotification" + item.UserId;
                            if (await _cache.KeyExistsAsync(lastNotiKey))
                            {
                                var lastNotiId = await _cache.StringGetAsync(lastNotiKey);
                                //随机取到了上条通知，就往后取一条
                                if (lastNotiId == item.Id.ToString() && curEmployeeNotis.Count > 1)
                                {
                                    if (index < curEmployeeNotis.Count - 1)
                                    {
                                        item = curEmployeeNotis[index + 1];
                                    }
                                }
                            }
                            await _channelWriter.WriteAsync(new NotificationMessage(item.UserId, item.Title, item.Content, g.Value));
                            //上条通知的ID
                            await _cache.StringSetAsync("LastNotification" + item.UserId, item.Id.ToString(), TimeSpan.FromMinutes(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NotificationJob发生错误");
            }
        }
    }
}