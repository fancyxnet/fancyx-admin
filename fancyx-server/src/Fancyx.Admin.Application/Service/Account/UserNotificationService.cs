using AutoMapper;
using Fancyx.Admin.Application.IService.Account;
using Fancyx.Admin.Application.IService.Account.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Account
{
    public class UserNotificationService : IUserNotificationService, IScopedDependency
    {
        private readonly IRepository<Notification> _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UserNotificationService(IRepository<Notification> repository, ICurrentUser currentUser, IMapper mapper)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserNotificationItem>> GetMyNotificationListAsync(GetMyNotificationListRequest req)
        {
            if (!_currentUser.Id.HasValue) return new PagedResult<UserNotificationItem>();

            var resp = await _repository
                .Where(x => x.UserId == _currentUser.Id)
                .WhereIf(!string.IsNullOrEmpty(req.Title), x => x.Title!.Contains(x.Title))
                .WhereIf(req.IsReaded.HasValue, x => x.IsReaded == req.IsReaded)
                .OrderBy(x => x.IsReaded)
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<UserNotificationItem>(req, resp.Total, _mapper.Map<List<Notification>, List<UserNotificationItem>>(resp.Items));
        }

        public async Task<UserNotificationNavbarInfo> GetMyNotificationNavbarInfoAsync()
        {
            var result = new UserNotificationNavbarInfo();
            if (!_currentUser.Id.HasValue) return result;

            var query = _repository.Where(x => x.UserId == _currentUser.Id);
            result.Items = await query.OrderBy(x => x.IsReaded).OrderByDescending(x => x.CreationTime)
                .Take(5).SelectToListAsync(x => new UserNotificationNavbarItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    IsReaded = x.IsReaded,
                    CreationTime = x.CreationTime,
                });
            result.NoReadedCount = await query.Where(x => !x.IsReaded).CountAsync();

            return result;
        }

        public async Task ReadedAsync(List<long> ids)
        {
            var now = DateTime.Now;
            await _repository.Where(x => x.UserId == _currentUser.Id && ids.Contains(x.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(f => f.IsReaded, true)
                .SetProperty(f => f.ReadedTime, now)
                .SetProperty(f => f.LastModifierId, _currentUser.Id)
                .SetProperty(f => f.LastModificationTime, now));
        }
    }
}