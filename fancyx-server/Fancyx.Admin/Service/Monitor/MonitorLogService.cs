using Fancyx.Admin.IService.Monitor;
using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.Core.Interfaces;
using Fancyx.Repository;
using Fancyx.Repository.Entities.Log;

namespace Fancyx.Admin.Service.Monitor
{
    public class MonitorLogService : IMonitorLogService, IScopedDependency
    {
        private readonly IRepository<ApiAccessLogDO> _apiAccessRepository;
        private readonly IRepository<ExceptionLogDO> _exceptionLogRepository;
        private readonly ICurrentUser _currentUser;

        public MonitorLogService(IRepository<ApiAccessLogDO> apiAccessRepository, IRepository<ExceptionLogDO> exceptionLogRepository, ICurrentUser currentUser)
        {
            _apiAccessRepository = apiAccessRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<ApiAccessLogListDto>> GetApiAccessLogListAsync(ApiAccessLogQueryDto dto)
        {
            var resp = await _apiAccessRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName != null && x.UserName.Contains(dto.UserName!))
                .WhereIf(!string.IsNullOrEmpty(dto.Path), x => x.Path.Contains(dto.Path!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ApiAccessLogListDto>(dto, resp.Total, resp.Items.MapperList<ApiAccessLogDO, ApiAccessLogListDto>());
        }

        public async Task<PagedResult<ExceptionLogListDto>> GetExceptionLogListAsync(ExceptionLogQueryDto dto)
        {
            var resp = await _exceptionLogRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName != null && x.UserName.Contains(dto.UserName!))
                .WhereIf(!string.IsNullOrEmpty(dto.Path), x => x.RequestPath != null && x.RequestPath.Contains(dto.Path!))
                .WhereIf(dto.IsHandled.HasValue, x => x.IsHandled == dto.IsHandled!)
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ExceptionLogListDto>(dto, resp.Total, resp.Items.MapperList<ExceptionLogDO, ExceptionLogListDto>());
        }

        public async Task HandleExceptionAsync(Guid exceptionId)
        {
            var entity = await _exceptionLogRepository.GetAsync(x => x.Id == exceptionId);
            if (entity == null) throw new EntityNotFoundException();
            entity.IsHandled = true;
            entity.HandledBy = _currentUser.UserName;
            entity.HandledTime = DateTime.Now;
            await _exceptionLogRepository.UpdateAsync(entity);
        }
    }
}