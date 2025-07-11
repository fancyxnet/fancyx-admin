﻿using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Repository;

namespace Fancyx.Admin.Service.System
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<NotificationDO> _repository;
        private readonly IRepository<EmployeeDO> _employeeRepository;

        public NotificationService(IRepository<NotificationDO> repository, IRepository<EmployeeDO> employeeRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
        }

        public Task AddNotificationAsync(NotificationDto dto)
        {
            var entity = new NotificationDO()
            {
                Title = dto.Title,
                Description = dto.Description,
                EmployeeId = dto.EmployeeId,
                IsReaded = false
            };
            return _repository.InsertAsync(entity);
        }

        public Task DeleteNotificationAsync(Guid[] ids)
        {
            return _repository.DeleteAsync(x => ids.Contains(x.Id));
        }

        public async Task<PagedResult<NotificationResultDto>> GetNotificationListAsync(NotificationSearchDto dto)
        {
            var list = await _repository.Select.From<EmployeeDO>().LeftJoin((n, e) => n.EmployeeId == e.Id)
                .WhereIf(!string.IsNullOrEmpty(dto.Title), (x, e) => x.Title!.Contains(x.Title))
                .WhereIf(dto.IsReaded.HasValue, (x, e) => x.IsReaded == dto.IsReaded)
                .Count(out var total)
                .Page(dto.Current, dto.PageSize)
                .ToListAsync((n, e) => new NotificationResultDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    EmployeeId = n.EmployeeId,
                    IsReaded = n.IsReaded,
                    CreationTime = n.CreationTime,
                    ReadedTime = n.ReadedTime,
                    EmployeeName = e.Name
                });
            return new PagedResult<NotificationResultDto>(dto, total, list);
        }

        public async Task UpdateNotificationAsync(NotificationDto dto)
        {
            var entity = await _repository.Where(x => x.Id == dto.Id).FirstAsync();
            if (entity.IsReaded)
            {
                throw new BusinessException(message: "已读消息不能修改");
            }
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.EmployeeId = dto.EmployeeId;
            await _repository.UpdateAsync(entity);
        }
    }
}