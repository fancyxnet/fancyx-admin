using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Core.Extensions;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.System;
using Fancyx.Logger;
using Fancyx.Shared.Consts;
using System.Linq;

namespace Fancyx.Admin.Service.System
{
    public class ConfigService : IConfigService
    {
        private readonly IRepository<Config> _configRepository;
        private readonly ConfigSharedService _configSharedService;

        public ConfigService(IRepository<Config> configRepository, ConfigSharedService configSharedService)
        {
            _configRepository = configRepository;
            _configSharedService = configSharedService;
        }

        public async Task AddConfigAsync(ConfigDto dto)
        {
            if (await _configRepository.AnyAsync(x => x.Key.ToLower() == dto.Key.ToLower()))
            {
                throw new BusinessException($"配置【{dto.Key}】已存在");
            }

            var entity = new Config()
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
            var resp = await _configRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.ToLower().Contains(dto.Name!.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(dto.Key), x => x.Key.ToLower().Contains(dto.Key!.ToLower()))
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<ConfigListDto>(resp.Total, resp.Items.MapperList<Config, ConfigListDto>());
        }

        public async Task DeleteConfigAsync(Guid id)
        {
            var entity = await _configRepository.GetAsync(x => x.Id == id);
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
            var entity = await _configRepository.GetAsync(x => x.Id == dto.Id);
            if (entity == null)
            {
                throw new BusinessException("数据不存在");
            }

            var key = dto.Key.ToLower();
            if (await _configRepository.AnyAsync(x => x.Key.ToLower() == key) && entity.Key.ToLower() != key)
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