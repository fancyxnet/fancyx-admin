using Fancyx.Core.Interfaces;
using Fancyx.EfCore.Aop;
using Fancyx.EfCore;
using Fancyx.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Shared.Logger;
using Fancyx.Admin.Application.IService.System.Models;
using AutoMapper;
using Fancyx.SnowflakeId;

namespace Fancyx.Admin.Application.Service.System;

public class DictTypeService : IDictTypeService
{
    private readonly IRepository<DictType> _dictTypeRepository;
    private readonly IRepository<DictData> _dictDataRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public DictTypeService(IRepository<DictType> dictTypeRepository, IRepository<DictData> dictDataRepository, ICurrentUser currentUser, IMapper mapper)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    [LogRecord(LogRecordConsts.DictType, LogRecordConsts.DictAddSubType, "{{Id}}", LogRecordConsts.DictAddContent)]
    public async Task AddDictTypeAsync(AddOrUpdateDictTypeRequest req)
    {
        if (await _dictTypeRepository.AnyAsync(x => x.Type.ToLower() == req.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        var entity = new DictType
        {
            Id = IdGenerater.Instance.NextId(),
            Name = req.Name,
            IsEnabled = req.IsEnabled,
            Type = req.DictType,
            Remark = req.Remark
        };

        LogRecordContext.PutVariable("Id", entity.Id.ToString());
        LogRecordContext.PutVariable("Name", entity.Name!);

        await _dictTypeRepository.InsertAsync(entity);
    }

    [Transactional]
    [LogRecord(LogRecordConsts.DictType, LogRecordConsts.DictDeleteSubType, "{{Id}}", LogRecordConsts.DictDeleteContent)]
    public async Task DeleteDictTypeAsync(string dictType)
    {
        var dict = await _dictTypeRepository.GetAsync(x => x.Type.ToLower() == dictType.ToLower()) ?? throw new EntityNotFoundException();
        await _dictDataRepository.DeleteAsync(x => x.DictType == dictType);
        await _dictTypeRepository.DeleteAsync(x => x.Type == dictType);

        LogRecordContext.PutVariable("Id", dict.Id.ToString());
        LogRecordContext.PutVariable("Name", dict.Name);
        LogRecordContext.PutVariable("DictType", dict.Type);
    }

    public async Task<PagedResult<DictTypeItem>> GetDictTypeListAsync(GetDictTypeListRequest req)
    {
        var resp = await _dictTypeRepository.GetQueryable()
            .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.Contains(req.Name!))
            .WhereIf(!string.IsNullOrEmpty(req.DictType), x => x.Type.Contains(req.DictType!))
            .OrderByDescending(x => x.CreationTime)
            .Select(x => new DictTypeItem
            {
                Name = x.Name,
                Id = x.Id,
                IsEnabled = x.IsEnabled,
                DictType = x.Type,
                Remark = x.Remark,
                CreationTime = x.CreationTime
            })
            .PagedAsync(req.Current, req.PageSize);
        return new PagedResult<DictTypeItem>(req)
        {
            TotalCount = resp.Total,
            Items = resp.Items
        };
    }

    [Transactional]
    public async Task UpdateDictTypeAsync(AddOrUpdateDictTypeRequest req)
    {
        var entity = await _dictTypeRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
        var isUpdateType = !entity.Type.Equals(req.DictType, StringComparison.CurrentCultureIgnoreCase);
        if (isUpdateType && await _dictTypeRepository.AnyAsync(x => x.Type.ToLower() == req.DictType.ToLower()))
        {
            throw new BusinessException(message: "字典类型已存在");
        }

        entity.Name = req.Name;
        entity.IsEnabled = req.IsEnabled;
        entity.Type = req.DictType;
        entity.Remark = req.Remark;

        await _dictTypeRepository.UpdateAsync(entity);
        if (isUpdateType)
        {
            await _dictDataRepository.Where(x => x.DictType == entity.Type)
                .ExecuteUpdateAsync(x => x.SetProperty(f => f.DictType, req.DictType)
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

    [Transactional]
    [LogRecord(LogRecordConsts.DictType, LogRecordConsts.DictBatchDeleteSubType, "{{Ids}}", LogRecordConsts.DictBatchDeleteContent)]
    public async Task DeleteDictTypesAsync(long[] ids)
    {
        var dictTypes = await _dictTypeRepository.Where(x => ids.Contains(x.Id)).SelectToListAsync(x => x.Type);
        await _dictDataRepository.DeleteAsync(x => dictTypes.Contains(x.DictType));
        await _dictTypeRepository.DeleteAsync(x => ids.Contains(x.Id));

        LogRecordContext.PutVariable("Ids", string.Join(',', ids));
    }

    public async Task<DictTypeItem> GetDictTypeAsync(long id)
    {
        var data = await _dictTypeRepository.GetAsync(x => x.Id == id) ?? throw new EntityNotFoundException();
        return _mapper.Map<DictTypeItem>(data);
    }
}