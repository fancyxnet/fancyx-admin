using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigService _configService;
        private readonly ConfigSharedService _configSharedService;

        public ConfigController(IConfigService configService, ConfigSharedService configSharedService)
        {
            _configService = configService;
            _configSharedService = configSharedService;
        }

        [HttpPost("Add")]
        [HasPermission("Sys.Config.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddConfigAsync([FromBody] AddOrUpdateConfigRequest req)
        {
            await _configService.AddConfigAsync(req);
            return Result.Data(true);
        }

        [HttpGet("List")]
        [HasPermission("Sys.Config.List")]
        public async Task<AppResponse<PagedResult<ConfigItem>>> GetConfigListAsync([FromQuery] GetConfigListRequest req)
        {
            var data = await _configService.GetConfigListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Sys.Config.Update")]
        [ApiAccessLog(operateName: "修改配置", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateConfigAsync(AddOrUpdateConfigRequest req)
        {
            await _configService.UpdateConfigAsync(req);
            return Result.Data(true);
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission("Sys.Config.Delete")]
        [ApiAccessLog(operateName: "删除配置", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteConfigAsync(long id)
        {
            await _configService.DeleteConfigAsync(id);
            return Result.Data(true);
        }

        [HttpGet("One")]
        public async Task<AppResponse<string>> GetConfigAsync(string key)
        {
            var value = await _configSharedService.GetAsync(key);
            return Result.Data(value);
        }

        [HttpGet("Group")]
        public async Task<AppResponse<Dictionary<string, string>>> GetConfigByGroupAsync(string group)
        {
            Dictionary<string, string> dictionary = await _configSharedService.GetGroupAsync(group);
            return Result.Data(dictionary);
        }

        [HttpGet("{id}")]
        public async Task<AppResponse<ConfigItem>> GetConfigAsync([FromRoute] long id)
        {
            var data = await _configService.GetConfigAsync(id);
            return Result.Data(data);
        }
    }
}