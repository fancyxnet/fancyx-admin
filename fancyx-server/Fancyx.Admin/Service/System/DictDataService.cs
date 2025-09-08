using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.Core.Helpers;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.System;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

namespace Fancyx.Admin.Service.System
{
    public class DictDataService : IDictDataService
    {
        private readonly IRepository<DictData> _dictRepository;

        public DictDataService(IRepository<DictData> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        public async Task<bool> AddDictDataAsync(DictDataDto dto)
        {
            var isExist = await _dictRepository.AnyAsync(x => x.Value.ToLower() == dto.Value.ToLower());
            if (isExist)
            {
                throw new BusinessException("字典值已存在");
            }
            var entity = AutoMapperHelper.Instance.Map<DictDataDto, DictData>(dto);
            await _dictRepository.InsertAsync(entity);

            return true;
        }

        [AsyncLogRecord(LogRecordConsts.SysDictData, LogRecordConsts.SysDictDataDeleteSubType, "{{ids}}", LogRecordConsts.SysDictDataDeleteContent)]
        public async Task<bool> DeleteDictDataAsync(Guid[] ids)
        {
            var entity = await _dictRepository.GetAsync(x => ids.Contains(x.Id))
                ?? throw new BusinessException("数据不存在");
            await _dictRepository.DeleteAsync(entity);

            LogRecordContext.PutVariable("ids", string.Join(',', ids));

            return true;
        }

        public async Task<PagedResult<DictDataListDto>> GetDictDataListAsync(DictDataQueryDto dto)
        {
            var resp = await _dictRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Label), x => x.Label != null && x.Label.Contains(dto.Label!))
                .WhereIf(!string.IsNullOrEmpty(dto.DictType), x => x.DictType != null && x.DictType.Contains(dto.DictType!))
                .OrderBy(x => x.Sort).OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<DictDataListDto>(resp.Total, resp.Items.MapperList<DictData, DictDataListDto>());
        }

        [AsyncLogRecord(LogRecordConsts.SysDictData, LogRecordConsts.SysDictDataUpdateSubType, "{{id}}", LogRecordConsts.SysDictDataUpdateContent)]
        public async Task<bool> UpdateDictDataAsync(DictDataDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _dictRepository.GetAsync(x => x.Id == dto.Id) ?? throw new BusinessException("数据不存在");
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