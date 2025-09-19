using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Logger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Sys.DictData.Add")]
        public async Task<AppResponse<bool>> AddDictDataAsync(DictDataDto dto)
        {
            await _dictService.AddDictDataAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 字典分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Sys.DictData.List")]
        public async Task<AppResponse<PagedResult<DictDataListDto>>> GetDictDataListAsync([FromQuery] DictDataQueryDto dto)
        {
            var data = await _dictService.GetDictDataListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [HasPermission("Sys.DictData.Update")]
        public async Task<AppResponse<bool>> UpdateDictDataAsync(DictDataDto dto)
        {
            await _dictService.UpdateDictDataAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [HasPermission("Sys.DictData.Delete")]
        [ApiAccessLog(operateName: "删除字典数据", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteDictDataAsync([FromBody] Guid[] ids)
        {
            await _dictService.DeleteDictDataAsync(ids);
            return Result.Ok();
        }
    }
}