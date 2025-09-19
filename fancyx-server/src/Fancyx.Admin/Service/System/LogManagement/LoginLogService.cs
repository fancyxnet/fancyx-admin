using AutoMapper;

using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.IService.System.LogManagement;
using Fancyx.Admin.IService.System.LogManagement.Dtos;
using Fancyx.EfCore;

namespace Fancyx.Admin.Service.System.LogManagement
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

        public async Task<PagedResult<LoginLogListDto>> GetLoginLogListAsync(LoginLogQueryDto dto)
        {
            var resp = await _loginLogRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName.Contains(dto.UserName!))
                .WhereIf(dto.Status == 1, x => x.IsSuccess)
                .WhereIf(dto.Status == 2, x => !x.IsSuccess)
                .WhereIf(!string.IsNullOrEmpty(dto.Address), x => x.Address != null && x.Address.Contains(dto.Address!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<LoginLogListDto>(resp.Total, _mapper.Map<List<LoginLog>, List<LoginLogListDto>>(resp.Items));
        }
    }
}