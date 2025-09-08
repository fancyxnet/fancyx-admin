using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.System;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

namespace Fancyx.Admin.Service.System;

public class DictTypeService : IDictTypeService
{
    private readonly IRepository<DictTypeDO> _dictTypeRepository;
    private readonly IRepository<DictDataDO> _dictDataRepository;

    public DictTypeService(IRepository<DictTypeDO> dictTypeRepository, IRepository<DictDataDO> dictDataRepository)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
    }

    [AsyncLogRecord(LogRecordConsts.SysDictType, LogRecordConsts.SysDictAddSubType, "{{dict.Id}}", LogRecordConsts.SysDictAddContent)]
    public async Task AddDictTypeAsync(DictTypeDto dto)
    {
        if (await _dictTypeRepository.AnyAsync(x => x.DictType.ToLower() == dto.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        var entity = new DictTypeDO
        {
            Name = dto.Name,
            IsEnabled = dto.IsEnabled,
            DictType = dto.DictType,
            Remark = dto.Remark
        };

        LogRecordContext.PutVariable("dict", entity);

        await _dictTypeRepository.InsertAsync(entity);
    }

    [AsyncLogRecord(LogRecordConsts.SysDictType, LogRecordConsts.SysDictDeleteSubType, "{{dict.Id}}", LogRecordConsts.SysDictDeleteContent)]
    public async Task DeleteDictTypeAsync(string dictType)
    {
        var dict = await _dictDataRepository.GetAsync(x => x.DictType.ToLower() == dictType.ToLower()) ?? throw new EntityNotFoundException();
        await _dictDataRepository.DeleteAsync(x => x.DictType == dictType);
        await _dictTypeRepository.DeleteAsync(x => x.DictType == dictType);

        LogRecordContext.PutVariable("dict", dict);
    }

    public async Task<PagedResult<DictTypeResultDto>> GetDictTypeListAsync(DictTypeSearchDto dto)
    {
        var resp = await _dictTypeRepository.GetQueryable()
            .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.Contains(dto.Name!))
            .WhereIf(!string.IsNullOrEmpty(dto.DictType), x => x.DictType.Contains(dto.DictType!))
            .OrderByDescending(x => x.CreationTime)
            .Select(x => new DictTypeResultDto
            {
                Name = x.Name,
                Id = x.Id,
                IsEnabled = x.IsEnabled,
                DictType = x.DictType,
                Remark = x.Remark,
                CreationTime = x.CreationTime
            })
            .PagedAsync(dto.Current, dto.PageSize);
        return new PagedResult<DictTypeResultDto>(dto)
        {
            TotalCount = resp.Total,
            Items = resp.Items
        };
    }

    public async Task UpdateDictTypeAsync(DictTypeDto dto)
    {
        var entity = await _dictTypeRepository.GetAsync(x => x.Id == dto.Id) ?? throw new EntityNotFoundException();
        if (!entity.DictType.Equals(dto.DictType, StringComparison.CurrentCultureIgnoreCase) 
            && await _dictTypeRepository.AnyAsync(x => x.DictType.ToLower() == dto.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        entity.Name = dto.Name;
        entity.IsEnabled = dto.IsEnabled;
        entity.DictType = dto.DictType;
        entity.Remark = dto.Remark;

        await _dictTypeRepository.UpdateAsync(entity);
    }

    public Task<List<AppOption>> GetDictDataOptionsAsync(string type)
    {
        return _dictDataRepository
            .Where(x => x.DictType == type)
            .OrderBy(x => x.Sort)
            .SelectToListAsync(x => new AppOption(x.Label, x.Value));
    }

    [AsyncLogRecord(LogRecordConsts.SysDictType, LogRecordConsts.SysDictBatchDeleteSubType, "{{ids}}", LogRecordConsts.SysDictBatchDeleteContent)]
    public async Task DeleteDictTypesAsync(Guid[] ids)
    {
        var dictTypes = await _dictTypeRepository.Where(x => ids.Contains(x.Id)).SelectToListAsync(x => x.DictType);
        await _dictDataRepository.DeleteAsync(x => dictTypes.Contains(x.DictType));
        await _dictTypeRepository.DeleteAsync(x => ids.Contains(x.Id));

        LogRecordContext.PutVariable("ids", string.Join(',', ids));
    }
}