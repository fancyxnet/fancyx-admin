using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerGroup("代码生成")]
    public class GenController : ControllerBase
    {
        private readonly IGenService _genService;

        public GenController(IGenService genService)
        {
            _genService = genService;
        }

        [HttpPost("GenCode")]
        [HasPermission("Sys.Gen.GenCode")]
        public async Task<AppResponse<GenCodeResponse>> GenCodeAsync(long tableId)
        {
            var data = await _genService.GenCodeAsync(tableId);
            return Result.Data(data);
        }

        [HttpPost("ImportTable")]
        [HasPermission("Sys.Gen.ImportTable")]
        public async Task<AppResponse<bool>> ImportTableAsync(string table)
        {
            await _genService.ImportTableAsync(table);
            return Result.Ok();
        }

        [HttpGet("GetTableList")]
        [HasPermission("Sys.Gen.GetTableList")]
        public async Task<AppResponse<PagedResult<TableInfoItem>>> GetTableListAsync([FromQuery] GetTableListRequest req)
        {
            var data = await _genService.GetTableListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("GenSyncFromDb")]
        [HasPermission("Sys.Gen.GenSyncFromDb")]
        public async Task<AppResponse<bool>> GenSyncFromDb(long tableId)
        {
            await _genService.GenSyncFromDb(tableId);
            return Result.Ok();
        }

        [HttpGet("GetGenTableList")]
        [HasPermission("Sys.Gen.GetGenTableList")]
        public async Task<AppResponse<PagedResult<GenTableItem>>> GetGenTableListAsync([FromQuery] GetGenTableListRequest req)
        {
            var data = await _genService.GetGenTableListAsync(req);
            return Result.Data(data);
        }

        [HttpGet("GetGenTableColumnList")]
        [HasPermission("Sys.Gen.GetGenTableColumnList")]
        public async Task<AppResponse<PagedResult<GenTableColumnItem>>> GetGenTableColumnListAsync([FromQuery] GenTableColumnRequest req)
        {
            var data = await _genService.GetGenTableColumnListAsync(req);
            return Result.Data(data);
        }

        [HttpDelete("DeleteGenTable/{tableId}")]
        [HasPermission("Sys.Gen.DeleteGenTable")]
        public async Task<AppResponse<bool>> DeleteGenTableAsync(long tableId)
        {
            await _genService.DeleteGenTableAsync(tableId);
            return Result.Ok();
        }

        [HttpPut("SaveGenTableInfo")]
        [HasPermission("Sys.Gen.SaveGenTableInfo")]
        public async Task<AppResponse<bool>> SaveGenTableInfoAsync([FromBody] SaveGenTableInfoRequest req)
        {
            await _genService.SaveGenTableInfoAsync(req);
            return Result.Ok();
        }

        [HttpPut("SaveGenColumnInfo")]
        [HasPermission("Sys.Gen.SaveGenColumnInfo")]
        public async Task<AppResponse<bool>> SaveGenColumnInfoAsync([FromBody] List<SaveGenColumnInfoItem> dtos)
        {
            await _genService.SaveGenColumnInfoAsync(dtos);
            return Result.Ok();
        }

        [HttpGet("GetGenDetailsInfo")]
        [HasPermission("Sys.Gen.GetGenDetailsInfo")]
        public async Task<AppResponse<GenDetails>> GetGenDetailsInfoAsync(long tableId)
        {
            var data = await _genService.GetGenDetailsInfoAsync(tableId);
            return Result.Data(data);
        }
    }
}
