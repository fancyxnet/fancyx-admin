using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Job;
using Fancyx.Job;

namespace Fancyx.Admin.Service.System
{
    public class ScheduledTaskService : IScheduledTaskService
    {
        private readonly IRepository<ScheduledTask> _scheduledTaskRepository;
        private readonly IRepository<TaskExecutionLog> _taskExecutionLogRepository;
        private readonly IJobControl _jobControl;

        public ScheduledTaskService(IRepository<ScheduledTask> scheduledTaskRepository, IRepository<TaskExecutionLog> taskExecutionLogRepository, IJobControl jobControl)
        {
            _scheduledTaskRepository = scheduledTaskRepository;
            _taskExecutionLogRepository = taskExecutionLogRepository;
            _jobControl = jobControl;
        }

        public Task AddAsync(ScheduledTaskDto dto)
        {
            return _jobControl.AddJobAsync(dto.TaskKey, dto.CronExpression, dto.Description, dto.IsActive);
        }

        public Task DeleteAsync(string key)
        {
            return _jobControl.DeleteJobAsync(key);
        }

        public async Task<PagedResult<TaskExecutionLogListDto>> GetExecutionLogListAsync(TaskExecutionLogQueryDto dto)
        {
            var resp = await _taskExecutionLogRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.TaskKey), x => x.TaskKey == dto.TaskKey)
                .WhereIf(dto.Status > 0, x => x.Status == dto.Status!.Value)
                .WhereIf(dto.ExecutionTimeRange != null && dto.ExecutionTimeRange.Length >= 2, x => x.ExecutionTime >= dto.ExecutionTimeRange![0] && x.ExecutionTime <= dto.ExecutionTimeRange![1])
                .WhereIf(dto.Cost > 0, x => x.Cost >= dto.Cost)
                .OrderByDescending(x => x.ExecutionTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<TaskExecutionLogListDto>(dto, resp.Total, resp.Items.MapperList<TaskExecutionLog, TaskExecutionLogListDto>());
        }

        public async Task<PagedResult<ScheduledTaskListDto>> GetListAsync(ScheduledTaskQueryDto dto)
        {
            var resp = await _scheduledTaskRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.TaskKey), x => x.TaskKey == dto.TaskKey)
                .WhereIf(!string.IsNullOrEmpty(dto.Description), x => x.Description != null && x.Description.Contains(dto.Description!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new ScheduledTaskListDto { Id = x.Id, CreationTime = x.CreationTime, CronExpression = x.CronExpression, Description = x.Description, IsActive = x.IsActive, LastModificationTime = x.LastModificationTime, TaskKey = x.TaskKey })
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ScheduledTaskListDto>(dto, resp.Total, resp.Items);
        }

        public async Task UpdateAsync(ScheduledTaskDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto.Id, nameof(dto));

            var entity = await _scheduledTaskRepository.FindAsync(dto.Id.Value) ?? throw new EntityNotFoundException();
            await _jobControl.UpdateJobAsync(entity.TaskKey, dto.TaskKey, dto.CronExpression, dto.Description, dto.IsActive);
        }
    }
}