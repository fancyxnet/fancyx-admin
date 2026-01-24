using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;

namespace Fancyx.Admin.Controllers.System;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DictTypeController : ControllerBase
{
    private readonly IDictTypeService _dictTypeService;

    public DictTypeController(IDictTypeService dictTypeService)
    {
        _dictTypeService = dictTypeService;
    }

    /// <summary>
    /// 新增字典类型
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("Add")]
    [HasPermission("Sys.DictType.Add")]
    public async Task<AppResponse<bool>> AddDictTypeAsync([FromBody] AddOrUpdateDictTypeRequest req)
    {
        await _dictTypeService.AddDictTypeAsync(req);
        return Result.Ok();
    }

    /// <summary>
    /// 分页查询字典类型列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet("List")]
    [HasPermission("Sys.DictType.List")]
    public async Task<AppResponse<PagedResult<DictTypeItem>>> GetDictTypeListAsync([FromQuery] GetDictTypeListRequest req)
    {
        var data = await _dictTypeService.GetDictTypeListAsync(req);
        return Result.Data(data);
    }

    /// <summary>
    /// 修改字典类型
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    [HasPermission("Sys.DictType.Update")]
    public async Task<AppResponse<bool>> UpdateDictTypeAsync([FromBody] AddOrUpdateDictTypeRequest req)
    {
        await _dictTypeService.UpdateDictTypeAsync(req);
        return Result.Ok();
    }

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="dictType"></param>
    /// <returns></returns>
    [HttpDelete("Delete/{dictType}")]
    [HasPermission("Sys.DictType.Delete")]
    [ApiAccessLog(operateName: "删除字典类型", operateType: [OperateType.Delete], reponseEnable: true)]
    public async Task<AppResponse<bool>> DeleteDictTypeAsync(string dictType)
    {
        await _dictTypeService.DeleteDictTypeAsync(dictType);
        return Result.Ok();
    }

    /// <summary>
    /// 字典选项
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet("Options")]
    public async Task<AppResponse<List<AppOption>>> GetDictDataOptionsAsync(string type)
    {
        var data = await _dictTypeService.GetDictDataOptionsAsync(type);
        return Result.Data(data);
    }

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete("DeleteMany")]
    [HasPermission("Sys.DictType.Delete")]
    [ApiAccessLog(operateName: "批量删除字典类型", operateType: [OperateType.Delete], reponseEnable: true)]
    public async Task<AppResponse<bool>> DeleteDictTypesAsync([FromBody] List<long> ids)
    {
        await _dictTypeService.DeleteDictTypesAsync(ids);
        return Result.Ok();
    }

    [HttpGet("{id}")]
    [HasPermission("Sys.DictType.List")]
    public async Task<AppResponse<DictTypeItem>> GetDictTypeAsync([FromRoute] long id)
    {
        var data = await _dictTypeService.GetDictTypeAsync(id);
        return Result.Data(data);
    }
}