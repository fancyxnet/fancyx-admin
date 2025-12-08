using AutoMapper;
using Fancyx.Admin.Application.IService.System.LogManagement;
using Fancyx.Admin.Application.IService.System.LogManagement.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;

namespace Fancyx.Admin.Application.Service.System.LogManagement
{
    public class LoginLogService : ILoginLogService
    {
        private readonly IRepository<LoginLog> _loginLogRepository;
        private readonly IMapper _mapper;

        public LoginLogService(IRepository<LoginLog> loginLogRepository, IMapper mapper)
        {
            _loginLogRepository = loginLogRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<LoginLogItem>> GetLoginLogListAsync(GetLoginLogListRequest req)
        {
            var resp = await _loginLogRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.UserName), x => x.UserName.Contains(req.UserName!))
                .WhereIf(req.Status == 1, x => x.IsSuccess)
                .WhereIf(req.Status == 2, x => !x.IsSuccess)
                .WhereIf(!string.IsNullOrEmpty(req.Address), x => x.Address != null && x.Address.Contains(req.Address!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);

            return new PagedResult<LoginLogItem>(resp.Total, _mapper.Map<List<LoginLog>, List<LoginLogItem>>(resp.Items));
        }
    }
}