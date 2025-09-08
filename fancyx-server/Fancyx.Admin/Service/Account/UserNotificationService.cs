using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.Account;
using Fancyx.Admin.IService.Account.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.Core.Interfaces;
using Fancyx.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.Account
{
    public class UserNotificationService : IUserNotificationService, IScopedDependency
    {
        private readonly IRepository<NotificationDO> _repository;
        private readonly IRepository<EmployeeDO> _employeeRepository;
        private readonly ICurrentUser _currentUser;

        public UserNotificationService(IRepository<NotificationDO> repository, IRepository<EmployeeDO> employeeRepository, ICurrentUser currentUser)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<UserNotificationListDto>> GetMyNotificationListAsync(UserNotificationQueryDto dto)
        {
            var employeeId = await this.GetCurrentEmployeeIdAsync();
            if (!employeeId.HasValue) return new PagedResult<UserNotificationListDto>();

            var resp = await _repository
                .Where(x => x.EmployeeId == employeeId)
                .WhereIf(!string.IsNullOrEmpty(dto.Title), x => x.Title!.Contains(x.Title))
                .WhereIf(dto.IsReaded.HasValue, x => x.IsReaded == dto.IsReaded)
                .OrderBy(x => x.IsReaded)
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<UserNotificationListDto>(dto, resp.Total, resp.Items.MapperList<NotificationDO, UserNotificationListDto>());
        }

        public async Task<UserNotificationNavbarDto> GetMyNotificationNavbarInfoAsync()
        {
            var result = new UserNotificationNavbarDto();
            var employeeId = await this.GetCurrentEmployeeIdAsync();
            if (!employeeId.HasValue) return result;

            var query = _repository.Where(x => x.EmployeeId == employeeId);
            result.Items = await query.OrderBy(x => x.IsReaded).OrderByDescending(x => x.CreationTime)
                .Take(5).SelectToListAsync(x => new UserNotificationNavbarItemDto
                {
                    Title = x.Title,
                    Content = x.Content,
                    IsReaded = x.IsReaded,
                    CreationTime = x.CreationTime,
                });
            result.NoReadedCount = (int)await query.Where(x => !x.IsReaded).CountAsync();

            return result;
        }

        public async Task ReadedAsync(Guid[] ids)
        {
            var employeeId = await this.GetCurrentEmployeeIdAsync();
            if (!employeeId.HasValue) return;

            var now = DateTime.Now;
            await _repository.Where(x => x.EmployeeId == employeeId && ids.Contains(x.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(f => f.IsReaded, true).SetProperty(f => f.ReadedTime, now));
        }

        private async Task<Guid?> GetCurrentEmployeeIdAsync()
        {
            return await _employeeRepository.Where(x => x.UserId.HasValue && x.UserId == _currentUser.Id).ToOneAsync(x => x.Id);
        }
    }
}