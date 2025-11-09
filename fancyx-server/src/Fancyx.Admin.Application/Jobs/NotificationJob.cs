using Coravel.Invocable;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core.AutoInject;
using Fancyx.EfCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RedLockNet.SERedis;

using StackExchange.Redis;

namespace Fancyx.Admin.Application.Jobs
{
    [DenpendencyInject(AsSelf = true)]
    public class NotificationJob : IInvocable
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly IRepository<Notification> _repository;
        private readonly MqttSharedService _mqttService;
        private readonly IDatabase _database;
        private readonly RedLockFactory _redLockFactory;

        public NotificationJob(ILogger<NotificationJob> logger, IRepository<Notification> repository, MqttSharedService mqttService, IDatabase database
            , RedLockFactory redLockFactory)
        {
            _logger = logger;
            _repository = repository;
            _mqttService = mqttService;
            _database = database;
            _redLockFactory = redLockFactory;
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
                            if (await _database.KeyExistsAsync(lastNotiKey))
                            {
                                var lastNotiId = await _database.StringGetAsync(lastNotiKey);
                                //随机取到了上条通知，就往后取一条
                                if (lastNotiId == item.Id.ToString() && curEmployeeNotis.Count > 1)
                                {
                                    if (index < curEmployeeNotis.Count - 1)
                                    {
                                        item = curEmployeeNotis[index + 1];
                                    }
                                }
                            }
                            var isSuc = await _mqttService.PushAsync("Notification:" + item.UserId, new { title = item.Title, content = item.Content, NoReadedCount = g.Value });
                            if (!isSuc) continue;
                            //上条通知的ID
                            await _database.StringSetAsync("LastNotification" + item.UserId, item.Id.ToString(), TimeSpan.FromMinutes(1));
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