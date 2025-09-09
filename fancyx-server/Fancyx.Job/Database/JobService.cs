using System.Reflection;

using Fancyx.Core.AutoInject;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Job;
using Fancyx.Job.Database.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Quartz;

namespace Fancyx.Job.Database
{
    [DenpendencyInject(AsSelf = true)]
    internal class JobService : IJobControl
    {
        private readonly IRepository<ScheduledTask> _scheduledTaskRepository;
        private readonly IRepository<TaskExecutionLog> _taskExecutionLogRepository;
        private readonly IScheduler _scheduler;
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public JobService(IRepository<ScheduledTask> scheduledTaskRepository, IRepository<TaskExecutionLog> taskExecutionLogRepository
            , IScheduler scheduler, IMemoryCache memoryCache, ICurrentUser currentUser, IUnitOfWorkManager unitOfWorkManager)
        {
            _scheduledTaskRepository = scheduledTaskRepository;
            _taskExecutionLogRepository = taskExecutionLogRepository;
            _scheduler = scheduler;
            _memoryCache = memoryCache;
            _currentUser = currentUser;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task AddJobAsync(string key, string cron, string? description, bool isActive = false)
        {
            if (!CronExpression.IsValidExpression(cron))
            {
                throw new FormatException("Cron表达式不正确");
            }
            if (await _scheduledTaskRepository.AnyAsync(x => x.TaskKey == key))
            {
                throw new InvalidOperationException($"任务KEY:{key}，已存在");
            }

            var entity = new ScheduledTask()
            {
                TaskKey = key,
                CronExpression = cron,
                Description = description,
                IsActive = isActive
            };
            await _scheduledTaskRepository.InsertAsync(entity);

            await this.ScheduleJobAsync(key, cron, isActive);
        }

        public async Task UpdateJobAsync(string oldKey, string key, string cron, string? description, bool isActive = false)
        {
            if (!CronExpression.IsValidExpression(cron))
            {
                throw new FormatException("Cron表达式不正确");
            }
            if (oldKey != key && await _scheduledTaskRepository.AnyAsync(x => x.TaskKey == key))
            {
                throw new InvalidOperationException($"任务KEY:{key}，已存在");
            }
            //删除旧Job
            await _scheduler.DeleteJob(new JobKey(JobKeyUtils.GetJobKey(oldKey)));
            //调度新Job
            await this.ScheduleJobAsync(key, cron, isActive);
            if (!isActive)
            {
                await _scheduler.PauseJob(new JobKey(JobKeyUtils.GetJobKey(key)));
            }

            await _scheduledTaskRepository.GetQueryable()
                .Where(e => e.TaskKey == oldKey)
                .ExecuteUpdateAsync(e => e.SetProperty(x => x.TaskKey, key)
                .SetProperty(x => x.CronExpression, cron)
                .SetProperty(x => x.IsActive, isActive)
                .SetProperty(x => x.Description, description)
                .SetProperty(x => x.LastModificationTime, DateTime.Now)
                .SetProperty(x => x.LastModifierId, _currentUser.Id));
        }

        public async Task DeleteJobAsync(string key)
        {
            using var uow = await _unitOfWorkManager.BeginAsync();
            try
            {
                await _taskExecutionLogRepository.DeleteAsync(x => x.TaskKey == key);
                await _scheduledTaskRepository.DeleteAsync(x => x.TaskKey == key);

                var jobMap = this.GetJobInfos();
                if (!jobMap.TryGetValue(key, out var taskType)) return;

                await _scheduler.DeleteJob(new JobKey(JobKeyUtils.GetJobKey(key)));
                await uow.CommitAsync();
            }
            catch (Exception)
            {
                await uow.RollbackAsync();
                throw;
            }
        }

        public Task<List<ScheduledTaskInfo>> GetScheduledTaskInfos()
        {
            return _scheduledTaskRepository.Where(x => x.IsActive)
                .Select(x => new ScheduledTaskInfo { TaskKey = x.TaskKey, CronExpression = x.CronExpression })
                .ToListAsync();
        }

        public async Task StartAsync(string taskKey)
        {
            var entity = await _scheduledTaskRepository.GetAsync(x => x.TaskKey == taskKey);
            if (entity != null)
            {
                var jobKey = new JobKey(JobKeyUtils.GetJobKey(taskKey));
                if (await _scheduler.CheckExists(jobKey))
                {
                    await _scheduler.ResumeJob(jobKey);
                }
                else
                {
                    await this.ScheduleJobAsync(taskKey, entity.CronExpression, true);
                }

                entity.IsActive = true;
                await _scheduledTaskRepository.UpdateAsync(entity);
            }
        }

        public async Task StopAsync(string taskKey)
        {
            var entity = await _scheduledTaskRepository.GetAsync(x => x.TaskKey == taskKey);
            if (entity != null)
            {
                var jobKey = new JobKey(JobKeyUtils.GetJobKey(taskKey));
                if (await _scheduler.CheckExists(jobKey))
                {
                    await _scheduler.PauseJob(jobKey);
                }

                entity.IsActive = false;
                await _scheduledTaskRepository.UpdateAsync(entity);
            }
        }

        public async Task TriggerJobAsync(string key)
        {
            var entity = await _scheduledTaskRepository.GetAsync(x => x.TaskKey == key);
            if (entity == null)
            {
                throw new InvalidOperationException("任务不存在");
            }

            var jobKey = new JobKey(JobKeyUtils.GetJobKey(key));
            if (!await _scheduler.CheckExists(jobKey))
            {
                await this.ScheduleJobAsync(key, entity.CronExpression, true);
                await _scheduler.PauseTrigger(new TriggerKey(JobKeyUtils.GetTriggerKey(key)));
            }
            await _scheduler.TriggerJob(jobKey);
        }

        private async Task ScheduleJobAsync(string taskKey, string cronExpression, bool throwEx = false)
        {
            var jobMap = this.GetJobInfos();
            if (!jobMap.TryGetValue(taskKey, out var taskType) || taskType == null)
            {
                if (throwEx)
                {
                    throw new InvalidOperationException($"未找到{taskKey}的定时任务执行类");
                }
                return;
            }

            var trigger = TriggerBuilder.Create().WithIdentity(JobKeyUtils.GetTriggerKey(taskKey))
                .WithCronSchedule(cronExpression).Build();
            var job = JobBuilder.Create(taskType).WithIdentity(JobKeyUtils.GetJobKey(taskKey)).Build();
            await _scheduler.ScheduleJob(job, trigger);
        }

        private Dictionary<string, TypeInfo> GetJobInfos()
        {
            return _memoryCache.Get<Dictionary<string, TypeInfo>>("JobTypeInfos") ?? [];
        }
    }
}