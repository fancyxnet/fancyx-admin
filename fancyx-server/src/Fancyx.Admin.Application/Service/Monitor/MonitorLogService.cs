using AutoMapper;

using Fancyx.Admin.Application.IService.Monitor;
using Fancyx.Admin.Application.IService.Monitor.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Shared.Logger.Entities;

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

        public async Task<PagedResult<ApiAccessLogItem>> GetApiAccessLogListAsync(GetApiAccessLogListRequest req)
        {
            var resp = await _apiAccessRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(req.UserName), x => x.UserName != null && x.UserName.Contains(req.UserName!))
                .WhereIf(!string.IsNullOrEmpty(req.Path), x => x.Path.Contains(req.Path!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<ApiAccessLogItem>(req, resp.Total, _mapper.Map<List<ApiAccessLog>, List<ApiAccessLogItem>>(resp.Items));
        }

        public async Task<PagedResult<ExceptionLogItem>> GetExceptionLogListAsync(GetExceptionLogListRequest req)
        {
            var resp = await _exceptionLogRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(req.UserName), x => x.UserName != null && x.UserName.Contains(req.UserName!))
                .WhereIf(!string.IsNullOrEmpty(req.Path), x => x.RequestPath != null && x.RequestPath.Contains(req.Path!))
                .WhereIf(req.IsHandled.HasValue, x => x.IsHandled == req.IsHandled!)
                .OrderBy(x => x.IsHandled)
                .ThenByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<ExceptionLogItem>(req, resp.Total, _mapper.Map<List<ExceptionLog>, List<ExceptionLogItem>>(resp.Items));
        }

        public async Task HandleExceptionAsync(long exceptionId)
        {
            var entity = await _exceptionLogRepository.FindAsync(exceptionId) ?? throw new EntityNotFoundException();
            entity.IsHandled = true;
            entity.HandledBy = _currentUser.UserName;
            entity.HandledTime = DateTime.Now;
            await _exceptionLogRepository.UpdateAsync(entity);
        }
    }
}