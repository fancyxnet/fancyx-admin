using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Job;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Collections.Concurrent;

namespace Fancyx.Job
{
    internal class GlobalJobListener : IJobListener
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BlockingCollection<TaskExecutionLogDO> _logQueue = [];
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public GlobalJobListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            this.ProcessQueue();
        }

        public string Name => "GlobalJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            context.Put("ExecutionTime", DateTime.Now);

            if (cancellationToken.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            var entity = new TaskExecutionLogDO()
            {
                TaskKey = JobKeyUtils.GetPureJobKey(context.JobDetail.Key.Name),
                ExecutionTime = (DateTime)context.Get("ExecutionTime")!,
                Status = jobException == null ? 1 : 2,
                Result = jobException?.Message ?? "成功",
            };
            entity.Cost = (int)Math.Round(DateTime.Now.Subtract(entity.ExecutionTime).TotalMilliseconds, 2);
            _logQueue.Add(entity, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
            return Task.CompletedTask;
        }

        private void ProcessQueue()
        {
            IRepository<TaskExecutionLogDO>? repository = _serviceProvider.GetService<IRepository<TaskExecutionLogDO>>();
            if (repository == null) return;

            Task.Run(async () =>
            {
                var batch = new List<TaskExecutionLogDO>();
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (_logQueue.TryTake(out var log, 500))
                    {
                        batch.Add(log);
                        if (batch.Count >= 500 || _logQueue.Count == 0)
                        {
                            await repository.InsertManyAsync(batch);
                            batch.Clear();
                        }
                    }
                }
            }, _cancellationTokenSource.Token);
        }
    }
}