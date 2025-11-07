using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
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
        public async Task<AppResponse<GenCodeResultDto>> GenCodeAsync(long tableId)
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
        public async Task<AppResponse<PagedResult<TableInfoDto>>> GetTableListAsync([FromQuery] GetTableQueryDto dto)
        {
            var data = await _genService.GetTableListAsync(dto);
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
        public async Task<AppResponse<PagedResult<GenTableListDto>>> GetGenTableListAsync([FromQuery] GenTableQueryDto dto)
        {
            var data = await _genService.GetGenTableListAsync(dto);
            return Result.Data(data);
        }

        [HttpGet("GetGenTableColumnList")]
        [HasPermission("Sys.Gen.GetGenTableColumnList")]
        public async Task<AppResponse<PagedResult<GenTableColumnListDto>>> GetGenTableColumnListAsync([FromQuery] GenTableColumnQueryDto dto)
        {
            var data = await _genService.GetGenTableColumnListAsync(dto);
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
        public async Task<AppResponse<bool>> SaveGenTableInfoAsync([FromBody] GenTableInfoDto dto)
        {
            await _genService.SaveGenTableInfoAsync(dto);
            return Result.Ok();
        }

        [HttpPut("SaveGenColumnInfo")]
        [HasPermission("Sys.Gen.SaveGenColumnInfo")]
        public async Task<AppResponse<bool>> SaveGenColumnInfoAsync([FromBody] List<GenTableColumnDto> dtos)
        {
            await _genService.SaveGenColumnInfoAsync(dtos);
            return Result.Ok();
        }

        [HttpGet("GetGenDetailsInfo")]
        [HasPermission("Sys.Gen.GetGenDetailsInfo")]
        public async Task<AppResponse<GenDetailsInfoDto>> GetGenDetailsInfoAsync(long tableId)
        {
            var data = await _genService.GetGenDetailsInfoAsync(tableId);
            return Result.Data(data);
        }
    }
}
