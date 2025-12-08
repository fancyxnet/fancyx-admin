using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.System
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _repository;
        private readonly IRepository<User> _userRepository;

        public NotificationService(IRepository<Notification> repository, IRepository<User> userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public Task AddNotificationAsync(AddOrUpdateNotificationRequest req)
        {
            var entity = new Notification()
            {
                Title = req.Title,
                Content = req.Content,
                UserId = req.UserId,
                IsReaded = false
            };
            return _repository.InsertAsync(entity);
        }

        public Task DeleteNotificationAsync(long[] ids)
        {
            return _repository.DeleteAsync(x => ids.Contains(x.Id));
        }

        public async Task<PagedResult<NotificationItem>> GetNotificationListAsync(GetNotificationListRequest req)
        {
            var resp = await _repository.GetQueryable().Join(_userRepository.GetQueryable().AsNoTracking(), n => n.UserId, u => u.Id, (n, u) =>
                new NotificationItem
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    UserId = n.UserId,
                    IsReaded = n.IsReaded,
                    CreationTime = n.CreationTime,
                    ReadedTime = n.ReadedTime,
                    NickName = u.NickName
                })
                .WhereIf(!string.IsNullOrEmpty(req.Title), u => u.Title!.Contains(u.Title))
                .WhereIf(req.IsReaded.HasValue, u => u.IsReaded == req.IsReaded)
                .OrderByDescending(u => u.CreationTime)
                .PagedAsync(req.Current, req.PageSize);
            return new PagedResult<NotificationItem>(req, resp.Total, resp.Items);
        }

        public async Task UpdateNotificationAsync(AddOrUpdateNotificationRequest req)
        {
            var entity = await _repository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            if (entity.IsReaded)
            {
                throw new BusinessException(message: "已读消息不能修改");
            }
            entity.Title = req.Title;
            entity.Content = req.Content;
            entity.UserId = req.UserId;
            await _repository.UpdateAsync(entity);
        }
    }
}