using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Cracker.EfCore;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger;

namespace Fancyx.Admin.Application.Service.System
{
    public class DictDataService : IDictDataService
    {
        private readonly IRepository<DictData> _dictDataRepository;
        private readonly IMapper _mapper;

        public DictDataService(IRepository<DictData> dictRepository, IMapper mapper)
        {
            _dictDataRepository = dictRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddDictDataAsync(AddOrUpdateDictDataRequest req)
        {
            var isExist = await _dictDataRepository.AnyAsync(x => x.DictType == req.DictType && x.Value.ToLower() == req.Value.ToLower());
            if (isExist)
            {
                throw new BusinessException("字典值已存在");
            }
            var entity = _mapper.Map<AddOrUpdateDictDataRequest, DictData>(req);
            await _dictDataRepository.InsertAsync(entity);

            return true;
        }

        [LogRecord(LogRecordConsts.DictData, LogRecordConsts.DictDataDeleteSubType, "{{Ids}}", LogRecordConsts.DictDataDeleteContent)]
        public async Task<bool> DeleteDictDataAsync(List<long> ids)
        {
            await _dictDataRepository.DeleteAsync(x => ids.Contains(x.Id));

            LogRecordContext.PutVariable("Ids", string.Join(',', ids));

            return true;
        }

        public async Task<PagedResult<DictDataItem>> GetDictDataListAsync(GetDictDataListRequest req)
        {
            var resp = await _dictDataRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Label), x => x.Label != null && x.Label.Contains(req.Label!))
                .WhereIf(!string.IsNullOrEmpty(req.DictType), x => x.DictType != null && x.DictType.Contains(req.DictType!))
                .OrderBy(x => x.Sort).ThenByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);

            return new PagedResult<DictDataItem>(resp.Total, _mapper.Map<List<DictData>, List<DictDataItem>>(resp.Items));
        }

        [LogRecord(LogRecordConsts.DictData, LogRecordConsts.DictDataUpdateSubType, "{{Id}}", LogRecordConsts.DictDataUpdateContent)]
        public async Task<bool> UpdateDictDataAsync(AddOrUpdateDictDataRequest req)
        {
            if (!req.Id.HasValue) throw new ArgumentNullException(nameof(req.Id));
            var entity = await _dictDataRepository.FindAsync(req.Id) ?? throw new BusinessException("数据不存在");
            var isExist = await _dictDataRepository.AnyAsync(x => x.Value.ToLower() == req.Value.ToLower());
            if (entity.Value.ToLower() != req.Value.ToLower() && isExist)
            {
                throw new BusinessException("字典值已存在");
            }

            entity.Value = req.Value;
            entity.DictType = req.DictType;
            entity.Label = req.Label;
            entity.Sort = req.Sort;
            entity.Remark = req.Remark;
            entity.IsEnabled = req.IsEnabled;
            await _dictDataRepository.UpdateAsync(entity);

            LogRecordContext.PutVariable("Id", entity.Id.ToString());
            LogRecordContext.PutVariable("Value", entity.Value);
            LogRecordContext.PutVariable("IsEnabled", entity.IsEnabled.ToString());
            return true;
        }

        public async Task<DictDataItem> GetDictDataAsync(long id)
        {
            var dictData = await _dictDataRepository.GetAsync(x => x.Id == id) ?? throw new EntityNotFoundException();
            return _mapper.Map<DictDataItem>(dictData);
        }
    }
}