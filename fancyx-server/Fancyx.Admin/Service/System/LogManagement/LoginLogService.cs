using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.System.LogManagement;
using Fancyx.Admin.IService.System.LogManagement.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.Repository;

namespace Fancyx.Admin.Service.System.LogManagement
{
    public class LoginLogService : ILoginLogService
    {
        private readonly IRepository<LoginLogDO> _loginLogRepository;

        public LoginLogService(IRepository<LoginLogDO> loginLogRepository)
        {
            _loginLogRepository = loginLogRepository;
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

            return new PagedResult<LoginLogListDto>(resp.Total, resp.Items.MapperList<LoginLogDO, LoginLogListDto>());
        }
    }
}