using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DictDataController : ControllerBase
    {
        private readonly IDictDataService _dictService;

        public DictDataController(IDictDataService dictService)
        {
            _dictService = dictService;
        }

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Sys.DictData.Add")]
        public async Task<AppResponse<bool>> AddDictDataAsync(AddOrUpdateDictDataRequest req)
        {
            await _dictService.AddDictDataAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 字典分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.DictData.List")]
        public async Task<AppResponse<PagedResult<DictDataItem>>> GetDictDataListAsync([FromQuery] GetDictDataListRequest req)
        {
            var data = await _dictService.GetDictDataListAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.DictData.Update")]
        public async Task<AppResponse<bool>> UpdateDictDataAsync(AddOrUpdateDictDataRequest req)
        {
            await _dictService.UpdateDictDataAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [HasPermission("Sys.DictData.Delete")]
        [ApiAccessLog(operateName: "删除字典数据", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteDictDataAsync([FromBody] long[] ids)
        {
            await _dictService.DeleteDictDataAsync(ids);
            return Result.Ok();
        }

        [HttpGet("{id}")]
        [HasPermission("Sys.DictData.List")]
        public async Task<AppResponse<DictDataItem>> GetDictDataAsync([FromRoute] long id)
        {
            var data = await _dictService.GetDictDataAsync(id);
            return Result.Data(data);
        }
    }
}