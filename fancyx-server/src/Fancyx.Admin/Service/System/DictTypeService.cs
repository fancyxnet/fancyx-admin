using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore.Aop;
using Fancyx.EfCore;
using Fancyx.Logger;
using Fancyx.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore;

namespace Fancyx.Admin.Service.System;

public class DictTypeService : IDictTypeService
{
    private readonly IRepository<DictType> _dictTypeRepository;
    private readonly IRepository<DictData> _dictDataRepository;
    private readonly ICurrentUser _currentUser;

    public DictTypeService(IRepository<DictType> dictTypeRepository, IRepository<DictData> dictDataRepository, ICurrentUser currentUser)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
        _currentUser = currentUser;
    }

    [AsyncLogRecord(LogRecordConsts.DictType, LogRecordConsts.DictAddSubType, "{{dict.Id}}", LogRecordConsts.DictAddContent)]
    public async Task AddDictTypeAsync(DictTypeDto dto)
    {
        if (await _dictTypeRepository.AnyAsync(x => x.Type.ToLower() == dto.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        var entity = new DictType
        {
            Name = dto.Name,
            IsEnabled = dto.IsEnabled,
            Type = dto.DictType,
            Remark = dto.Remark
        };

        LogRecordContext.PutVariable("dict", entity);

        await _dictTypeRepository.InsertAsync(entity);
    }

    [AsyncTransactional]
    [AsyncLogRecord(LogRecordConsts.DictType, LogRecordConsts.DictDeleteSubType, "{{dict.Id}}", LogRecordConsts.DictDeleteContent)]
    public async Task DeleteDictTypeAsync(string dictType)
    {
        var dict = await _dictDataRepository.GetAsync(x => x.DictType.ToLower() == dictType.ToLower()) ?? throw new EntityNotFoundException();
        await _dictDataRepository.DeleteAsync(x => x.DictType == dictType);
        await _dictTypeRepository.DeleteAsync(x => x.Type == dictType);

        LogRecordContext.PutVariable("dict", dict);
    }

    public async Task<PagedResult<DictTypeResultDto>> GetDictTypeListAsync(DictTypeSearchDto dto)
    {
        var resp = await _dictTypeRepository.GetQueryable()
            .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.Contains(dto.Name!))
            .WhereIf(!string.IsNullOrEmpty(dto.DictType), x => x.Type.Contains(dto.DictType!))
            .OrderByDescending(x => x.CreationTime)
            .Select(x => new DictTypeResultDto
            {
                Name = x.Name,
                Id = x.Id,
                IsEnabled = x.IsEnabled,
                DictType = x.Type,
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

    [AsyncTransactional]
    public async Task UpdateDictTypeAsync(DictTypeDto dto)
    {
        var entity = await _dictTypeRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
        var isUpdateType = !entity.Type.Equals(dto.DictType, StringComparison.CurrentCultureIgnoreCase);
        if (isUpdateType && await _dictTypeRepository.AnyAsync(x => x.Type.ToLower() == dto.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        entity.Name = dto.Name;
        entity.IsEnabled = dto.IsEnabled;
        entity.Type = dto.DictType;
        entity.Remark = dto.Remark;

        await _dictTypeRepository.UpdateAsync(entity);
        if (isUpdateType)
        {
            await _dictDataRepository.Where(x => x.DictType == entity.Type)
                .ExecuteUpdateAsync(x => x.SetProperty(f => f.DictType, dto.DictType)
                .SetProperty(f => f.LastModifierId, _currentUser.Id)
                .SetProperty(f => f.LastModificationTime, DateTime.Now));
        }
    }

    public Task<List<AppOption>> GetDictDataOptionsAsync(string type)
    {
        return _dictDataRepository
            .Where(x => x.DictType == type)
            .OrderBy(x => x.Sort)
            .SelectToListAsync(x => new AppOption(x.Label, x.Value));
    }

    [AsyncTransactional]
    [AsyncLogRecord(LogRecordConsts.DictType, LogRecordConsts.DictBatchDeleteSubType, "{{ids}}", LogRecordConsts.DictBatchDeleteContent)]
    public async Task DeleteDictTypesAsync(Guid[] ids)
    {
        var dictTypes = await _dictTypeRepository.Where(x => ids.Contains(x.Id)).SelectToListAsync(x => x.Type);
        await _dictDataRepository.DeleteAsync(x => dictTypes.Contains(x.DictType));
        await _dictTypeRepository.DeleteAsync(x => ids.Contains(x.Id));

        LogRecordContext.PutVariable("ids", string.Join(',', ids));
    }
}