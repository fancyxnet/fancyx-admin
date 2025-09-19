using System.Reflection;

using Fancyx.Core;
using Fancyx.Job.Database;
using Fancyx.Utils;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

using Quartz;

namespace Fancyx.Job
{
    internal class JobInitHostService : IHostedService
    {
        private readonly IScheduler _scheduler;
        private readonly JobService _jobService;
        private readonly IMemoryCache _memoryCache;

        public JobInitHostService(IScheduler scheduler, JobService jobService, IMemoryCache memoryCache)
        {
            _scheduler = scheduler;
            _jobService = jobService;
            _memoryCache = memoryCache;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //加载实现IJob接口的任务类
            var baseJobType = typeof(IJob);
            var jobClasses = ReflectionUtils.AllAssemblies.SelectMany(x => x.DefinedTypes.Where(t => !t.IsSealed && !t.IsAbstract && t.IsClass && t.IsAssignableTo(baseJobType))).ToList();
            var jobClassMap = new Dictionary<string, TypeInfo>();
            foreach (var jobType in jobClasses)
            {
                var jobKeyAttr = jobType.GetCustomAttribute<JobKeyAttribute>();
                if (jobKeyAttr == null || string.IsNullOrWhiteSpace(jobKeyAttr.Key)) continue;

                if (jobClassMap.ContainsKey(jobKeyAttr.Key))
                {
                    throw new InvalidOperationException($"任务KEY:{jobKeyAttr.Key}，已存在");
                }
                jobClassMap[jobKeyAttr.Key] = jobType;
            }
            if (jobClasses.Count > 0)
            {
                _memoryCache.Set("JobTypeInfos", jobClassMap);
            }
            var jobInfos = await _jobService.GetScheduledTaskInfos();
            if (jobInfos.Count <= 0) return;
            //调度已启用任务
            foreach (var jobInfo in jobInfos)
            {
                var exist = jobClassMap.TryGetValue(jobInfo.TaskKey!, out var taskType) && taskType != null;
                if (!exist) continue;

                var trigger = TriggerBuilder.Create().WithIdentity(JobKeyUtils.GetTriggerKey(jobInfo.TaskKey))
                    .WithCronSchedule(jobInfo.CronExpression).Build();
                var job = JobBuilder.Create(taskType!).WithIdentity(JobKeyUtils.GetJobKey(jobInfo.TaskKey)).Build();
                await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}