using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger;

namespace Fancyx.Admin.Application.Service.System
{
    public class DictDataService : IDictDataService
    {
        private readonly IRepository<DictData> _dictRepository;
        private readonly IMapper _mapper;

        public DictDataService(IRepository<DictData> dictRepository, IMapper mapper)
        {
            _dictRepository = dictRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddDictDataAsync(AddOrUpdateDictDataRequest dto)
        {
            var isExist = await _dictRepository.AnyAsync(x => x.Value.ToLower() == dto.Value.ToLower());
            if (isExist)
            {
                throw new BusinessException("字典值已存在");
            }
            var entity = _mapper.Map<AddOrUpdateDictDataRequest, DictData>(dto);
            await _dictRepository.InsertAsync(entity);

            return true;
        }

        [AsyncLogRecord(LogRecordConsts.DictData, LogRecordConsts.DictDataDeleteSubType, "{{ids}}", LogRecordConsts.DictDataDeleteContent)]
        public async Task<bool> DeleteDictDataAsync(long[] ids)
        {
            await _dictRepository.DeleteAsync(x => ids.Contains(x.Id));

            LogRecordContext.PutVariable("ids", string.Join(',', ids));

            return true;
        }

        public async Task<PagedResult<DictDataItem>> GetDictDataListAsync(GetDictDataListRequest dto)
        {
            var resp = await _dictRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Label), x => x.Label != null && x.Label.Contains(dto.Label!))
                .WhereIf(!string.IsNullOrEmpty(dto.DictType), x => x.DictType != null && x.DictType.Contains(dto.DictType!))
                .OrderBy(x => x.Sort).OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<DictDataItem>(resp.Total, _mapper.Map<List<DictData>, List<DictDataItem>>(resp.Items));
        }

        [AsyncLogRecord(LogRecordConsts.DictData, LogRecordConsts.DictDataUpdateSubType, "{{id}}", LogRecordConsts.DictDataUpdateContent)]
        public async Task<bool> UpdateDictDataAsync(AddOrUpdateDictDataRequest dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _dictRepository.FindAsync(dto.Id) ?? throw new BusinessException("数据不存在");
            var isExist = await _dictRepository.AnyAsync(x => x.Value.ToLower() == dto.Value.ToLower());
            if (entity.Value.ToLower() != dto.Value.ToLower() && isExist)
            {
                throw new BusinessException("字典值已存在");
            }

            entity.Value = dto.Value;
            entity.DictType = dto.DictType;
            entity.Label = dto.Label;
            entity.Sort = dto.Sort;
            entity.Remark = dto.Remark;
            entity.IsEnabled = dto.IsEnabled;
            await _dictRepository.UpdateAsync(entity);

            LogRecordContext.PutVariable("id", entity.Id);
            LogRecordContext.PutVariable("after", entity);
            return true;
        }
    }
}