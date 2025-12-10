using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger;

using System.Linq;

namespace Fancyx.Admin.Application.Service.System
{
    public class ConfigService : IConfigService
    {
        private readonly IRepository<Config> _configRepository;
        private readonly ConfigSharedService _configSharedService;
        private readonly IMapper _mapper;

        public ConfigService(IRepository<Config> configRepository, ConfigSharedService configSharedService, IMapper mapper)
        {
            _configRepository = configRepository;
            _configSharedService = configSharedService;
            _mapper = mapper;
        }

        public async Task AddConfigAsync(AddOrUpdateConfigRequest req)
        {
            if (await _configRepository.AnyAsync(x => x.Key.ToLower() == req.Key.ToLower()))
            {
                throw new BusinessException($"配置【{req.Key}】已存在");
            }

            var entity = new Config()
            {
                Name = req.Name,
                Key = req.Key!,
                Value = req.Value!,
                GroupKey = req.GroupKey,
                Remark = req.Remark
            };
            await _configRepository.InsertAsync(entity);
        }

        public async Task<PagedResult<ConfigItem>> GetConfigListAsync(GetConfigListRequest req)
        {
            var resp = await _configRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.ToLower().Contains(req.Name!.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(req.Key), x => x.Key.ToLower().Contains(req.Key!.ToLower()))
                .PagedAsync(req.Current, req.PageSize);

            return new PagedResult<ConfigItem>(resp.Total, _mapper.Map<List<Config>, List<ConfigItem>>(resp.Items));
        }

        public async Task DeleteConfigAsync(long id)
        {
            var entity = await _configRepository.FindAsync(id);
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

        [AsyncLogRecord(LogRecordConsts.Config, LogRecordConsts.ConfigUpdateSubType, "{{id}}", LogRecordConsts.ConfigUpdateContent)]
        public async Task UpdateConfigAsync(AddOrUpdateConfigRequest req)
        {
            var entity = await _configRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();

            var key = req.Key.ToLower();
            if (await _configRepository.AnyAsync(x => x.Key.ToLower() == key) && entity.Key.ToLower() != key)
            {
                throw new BusinessException($"配置【{req.Key}】已存在");
            }

            entity.Key = req.Key;
            entity.Value = req.Value;
            entity.GroupKey = req.GroupKey;
            entity.Name = req.Name;
            entity.Remark = req.Remark;

            await _configRepository.UpdateAsync(entity);

            _configSharedService.ClearCache(req.Key!);
            if (!string.IsNullOrEmpty(entity.GroupKey))
            {
                _configSharedService.ClearGroupCache(entity.GroupKey);
            }

            LogRecordContext.PutVariable("id", entity.Id);
            LogRecordContext.PutVariable("after", entity);
        }

        public async Task<ConfigItem> GetConfigAsync(long id)
        {
            var config = await _configRepository.GetAsync(x => x.Id == id) ?? throw new EntityNotFoundException();
            return _mapper.Map<ConfigItem>(config);
        }
    }
}