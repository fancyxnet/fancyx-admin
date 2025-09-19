using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.System
{
    public interface IScheduledTaskService : IScopedDependency
    {
        Task AddAsync(ScheduledTaskDto dto);

        Task<PagedResult<ScheduledTaskListDto>> GetListAsync(ScheduledTaskQueryDto dto);

        Task UpdateAsync(ScheduledTaskDto dto);

        Task DeleteAsync(string key);

        Task<PagedResult<TaskExecutionLogListDto>> GetExecutionLogListAsync(TaskExecutionLogQueryDto dto);
    }
}