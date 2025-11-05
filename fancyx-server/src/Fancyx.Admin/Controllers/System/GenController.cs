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
    }
}
