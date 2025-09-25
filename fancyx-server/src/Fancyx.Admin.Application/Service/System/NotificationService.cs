using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
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

        public Task AddNotificationAsync(NotificationDto dto)
        {
            var entity = new Notification()
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = dto.UserId,
                IsReaded = false
            };
            return _repository.InsertAsync(entity);
        }

        public Task DeleteNotificationAsync(long[] ids)
        {
            return _repository.DeleteAsync(x => ids.Contains(x.Id));
        }

        public async Task<PagedResult<NotificationResultDto>> GetNotificationListAsync(NotificationQueryDto dto)
        {
            var resp = await _repository.GetQueryable().Join(_userRepository.GetQueryable().AsNoTracking(), n => n.UserId, u => u.Id, (n, u) =>
                new NotificationResultDto
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
                .WhereIf(!string.IsNullOrEmpty(dto.Title), u => u.Title!.Contains(u.Title))
                .WhereIf(dto.IsReaded.HasValue, u => u.IsReaded == dto.IsReaded)
                .OrderByDescending(u => u.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<NotificationResultDto>(dto, resp.Total, resp.Items);
        }

        public async Task UpdateNotificationAsync(NotificationDto dto)
        {
            var entity = await _repository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            if (entity.IsReaded)
            {
                throw new BusinessException(message: "已读消息不能修改");
            }
            entity.Title = dto.Title;
            entity.Content = dto.Content;
            entity.UserId = dto.UserId;
            await _repository.UpdateAsync(entity);
        }
    }
}