using AutoMapper;
using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Admin.EfCore;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Logger.Entities;

namespace Fancyx.Admin.Application.Service.Monitor
{
    public class MonitorLogService : IMonitorLogService, IScopedDependency
    {
        private readonly IRepository<ApiAccessLog> _apiAccessRepository;
        private readonly IRepository<ExceptionLog> _exceptionLogRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public MonitorLogService(IRepository<ApiAccessLog> apiAccessRepository, IRepository<ExceptionLog> exceptionLogRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _apiAccessRepository = apiAccessRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PagedResult<ApiAccessLogListDto>> GetApiAccessLogListAsync(ApiAccessLogQueryDto dto)
        {
            var resp = await _apiAccessRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName != null && x.UserName.Contains(dto.UserName!))
                .WhereIf(!string.IsNullOrEmpty(dto.Path), x => x.Path.Contains(dto.Path!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ApiAccessLogListDto>(dto, resp.Total, _mapper.Map<List<ApiAccessLog>, List<ApiAccessLogListDto>>(resp.Items));
        }

        public async Task<PagedResult<ExceptionLogListDto>> GetExceptionLogListAsync(ExceptionLogQueryDto dto)
        {
            var resp = await _exceptionLogRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName != null && x.UserName.Contains(dto.UserName!))
                .WhereIf(!string.IsNullOrEmpty(dto.Path), x => x.RequestPath != null && x.RequestPath.Contains(dto.Path!))
                .WhereIf(dto.IsHandled.HasValue, x => x.IsHandled == dto.IsHandled!)
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ExceptionLogListDto>(dto, resp.Total, _mapper.Map<List<ExceptionLog>, List<ExceptionLogListDto>>(resp.Items));
        }

        public async Task HandleExceptionAsync(Guid exceptionId)
        {
            var entity = await _exceptionLogRepository.FindAsync(exceptionId) ?? throw new EntityNotFoundException();
            entity.IsHandled = true;
            entity.HandledBy = _currentUser.UserName;
            entity.HandledTime = DateTime.Now;
            await _exceptionLogRepository.UpdateAsync(entity);
        }
    }
}