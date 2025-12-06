using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
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

        public async Task AddConfigAsync(AddOrUpdateConfigRequest dto)
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

        public async Task<PagedResult<ConfigListDto>> GetConfigListAsync(GetConfigListRequest dto)
        {
            var resp = await _configRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.ToLower().Contains(dto.Name!.ToLower()))
                .WhereIf(!string.IsNullOrEmpty(dto.Key), x => x.Key.ToLower().Contains(dto.Key!.ToLower()))
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<ConfigListDto>(resp.Total, _mapper.Map<List<Config>, List<ConfigListDto>>(resp.Items));
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
        public async Task UpdateConfigAsync(AddOrUpdateConfigRequest dto)
        {
            var entity = await _configRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();

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