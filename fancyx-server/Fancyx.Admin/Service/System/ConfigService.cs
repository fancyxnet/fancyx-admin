using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Logger;
using Fancyx.Repository;
using Fancyx.Shared.Consts;

namespace Fancyx.Admin.Service.System
{
    public class ConfigService : IConfigService
    {
        private readonly IRepository<ConfigDO> _configRepository;
        private readonly ConfigSharedService _configSharedService;

        public ConfigService(IRepository<ConfigDO> configRepository, ConfigSharedService configSharedService)
        {
            _configRepository = configRepository;
            _configSharedService = configSharedService;
        }

        public async Task AddConfigAsync(ConfigDto dto)
        {
            if (_configRepository.Select.Any(x => x.Key.ToLower() == dto.Key.ToLower()))
            {
                throw new BusinessException($"配置【{dto.Key}】已存在");
            }

            var entity = new ConfigDO()
            {
                Name = dto.Name,
                Key = dto.Key!,
                Value = dto.Value!,
                GroupKey = dto.GroupKey,
                Remark = dto.Remark
            };
            await _configRepository.InsertAsync(entity);
        }

        public async Task<PagedResult<ConfigListDto>> GetConfigListAsync(ConfigQueryDto dto)
        {
            var rows = await _configRepository.Select
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.ToLower().Contains(dto.Name!.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(dto.Key), x => x.Key.ToLower().Contains(dto.Key!.ToLower()))
                .OrderByDescending(string.IsNullOrEmpty(dto.SortProperty), x => x.CreationTime)
                .OrderByPropertyName(dto.SortProperty, dto.IsAsecending)
                .Count(out var total)
                .Page(dto.Current, dto.PageSize)
                .ToListAsync<ConfigListDto>();

            return new PagedResult<ConfigListDto>(total, rows);
        }

        public async Task DeleteConfigAsync(Guid id)
        {
            var entity = _configRepository.Select.Where(x => x.Id == id).ToOne();
            if (entity == null)
            {
                throw new BusinessException("数据已删除");
            }

            await _configRepository.DeleteAsync(entity);

            _configSharedService.ClearCache(entity.Key!);
            if (!string.IsNullOrEmpty(entity.GroupKey))
            {
                _configSharedService.ClearGroupCache(entity.GroupKey);
            }
        }

        [AsyncLogRecord(LogRecordConsts.SysConfig, LogRecordConsts.SysConfigUpdateSubType, "{{id}}", LogRecordConsts.SysConfigUpdateContent)]
        public async Task UpdateConfigAsync(ConfigDto dto)
        {
            var entity = _configRepository.Select.Where(x => x.Id == dto.Id).ToOne();
            if (entity == null)
            {
                throw new BusinessException("数据不存在");
            }

            var key = dto.Key.ToLower();
            if (_configRepository.Select.Any(x => x.Key.ToLower() == key) && entity.Key.ToLower() != key)
            {
                throw new BusinessException($"配置【{dto.Key}】已存在");
            }

            entity.Key = dto.Key;
            entity.Value = dto.Value;
            entity.GroupKey = dto.GroupKey;
            entity.Name = dto.Name;
            entity.Remark = dto.Remark;

            await _configRepository.UpdateAsync(entity);

            _configSharedService.ClearCache(dto.Key!);
            if (!string.IsNullOrEmpty(entity.GroupKey))
            {
                _configSharedService.ClearGroupCache(entity.GroupKey);
            }

            LogRecordContext.PutVariable("id", entity.Id);
            LogRecordContext.PutVariable("after", entity);
        }
    }
}