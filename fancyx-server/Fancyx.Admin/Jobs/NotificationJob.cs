using Fancyx.Admin.Entities.System;
using Fancyx.Admin.SharedService;
using Fancyx.Core.AutoInject;
using Fancyx.Job;
using Fancyx.Repository;
using Microsoft.EntityFrameworkCore;
using Quartz;

using RedLockNet.SERedis;

using StackExchange.Redis;

namespace Fancyx.Admin.Jobs
{
    [DenpendencyInject(AsSelf = true)]
    [JobKey("NotificationJob")]
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly IRepository<NotificationDO> _repository;
        private readonly MqttSharedService _mqttService;
        private readonly IDatabase _database;
        private readonly RedLockFactory redLockFactory;

        public NotificationJob(ILogger<NotificationJob> logger, IRepository<NotificationDO> repository, MqttSharedService mqttService, IDatabase database
            , RedLockFactory redLockFactory)
        {
            _logger = logger;
            _repository = repository;
            _mqttService = mqttService;
            _database = database;
            this.redLockFactory = redLockFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var expiry = TimeSpan.FromSeconds(30);
                var wait = TimeSpan.FromSeconds(10);
                var retry = TimeSpan.FromSeconds(1);

                using var redLock = await redLockFactory.CreateLockAsync(nameof(NotificationJob), expiry, wait, retry);
                if (redLock.IsAcquired)
                {
                    var notis = await _repository.Where(x => !x.IsReaded).ToListAsync();
                    var groupMap = notis.GroupBy(x => x.EmployeeId).ToDictionary(k => k.Key, v => v.Count());
                    var random = new Random();
                    if (notis.Count > 0)
                    {
                        foreach (var g in groupMap)
                        {
                            var curEmployeeNotis = notis.Where(x => x.EmployeeId == g.Key).ToList();
                            var index = random.Next(0, curEmployeeNotis.Count);
                            var item = curEmployeeNotis[index];
                            var lastNotiKey = "LastNotification" + item.EmployeeId;
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
                            var isSuc = await _mqttService.PushAsync("Notification:" + item.EmployeeId, new { title = item.Title, content = item.Content, NoReadedCount = g.Value });
                            if (!isSuc) continue;
                            //上条通知的ID
                            await _database.StringSetAsync("LastNotification" + item.EmployeeId, item.Id.ToString(), TimeSpan.FromMinutes(1));
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