using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Dtos;
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
        public async Task<AppResponse<GenCodeResultDto>> GenCodeAsync(long tableId)
        {
            var data = await _genService.GenCodeAsync(tableId);
            return Result.Data(data);
        }

        [HttpPost("ImportTable")]
        public async Task<AppResponse<bool>> ImportTableAsync(string table)
        {
            await _genService.ImportTableAsync(table);
            return Result.Ok();
        }

        [HttpGet("GetTableList")]
        public async Task<AppResponse<PagedResult<TableInfoDto>>> GetTableListAsync([FromQuery] GetTableQueryDto dto)
        {
            var data = await _genService.GetTableListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("GenSyncFromDb")]
        public async Task<AppResponse<bool>> GenSyncFromDb(long tableId)
        {
            await _genService.GenSyncFromDb(tableId);
            return Result.Ok();
        }

        [HttpGet("GetGenTableList")]
        public async Task<AppResponse<PagedResult<GenTableListDto>>> GetGenTableListAsync([FromQuery] GenTableQueryDto dto)
        {
            var data = await _genService.GetGenTableListAsync(dto);
            return Result.Data(data);
        }

        [HttpGet("GetGenTableListColumnList")]
        public async Task<AppResponse<PagedResult<GenTableListColumnDto>>> GetGenTableListColumnListAsync([FromQuery] GenTableListColumnQueryDto dto)
        {
            var data = await _genService.GetGenTableListColumnListAsync(dto);
            return Result.Data(data);
        }

        [HttpDelete("DeleteGenTable/{tableId}")]
        public async Task<AppResponse<bool>> DeleteGenTableAsync(long tableId)
        {
            await _genService.DeleteGenTableAsync(tableId);
            return Result.Ok();
        }

        [HttpPut("SaveGenTableInfo")]
        public async Task<AppResponse<bool>> SaveGenTableInfoAsync([FromBody] GenTableInfoDto dto)
        {
            await _genService.SaveGenTableInfoAsync(dto);
            return Result.Ok();
        }

        [HttpPut("SaveGenColumnInfo")]
        public async Task<AppResponse<bool>> SaveGenColumnInfoAsync([FromBody] List<GenTableColumnDto> dtos)
        {
            await _genService.SaveGenColumnInfoAsync(dtos);
            return Result.Ok();
        }

        [HttpGet("GetGenDetailsInfo")]
        public async Task<AppResponse<GenDetailsInfoDto>> GetGenDetailsInfoAsync(long tableId)
        {
            var data = await _genService.GetGenDetailsInfoAsync(tableId);
            return Result.Data(data);
        }
    }
}
