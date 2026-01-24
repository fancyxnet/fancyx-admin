using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Sys.Menu.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddMenuAsync([FromBody] AddOrUpdateMenuRequest req)
        {
            await _menuService.AddMenuAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 菜单树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.Menu.List")]
        [ApiAccessLog(operateType: [OperateType.Query])]
        public async Task<AppResponse<List<MenuItem>>> GetMenuListAsync([FromQuery] GetMenuListRequest req)
        {
            var data = await _menuService.GetMenuListAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.Menu.Update")]
        [ApiAccessLog(operateName: "修改菜单", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateMenuAsync([FromBody] AddOrUpdateMenuRequest req)
        {
            await _menuService.UpdateMenuAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [HasPermission("Sys.Menu.Delete")]
        [ApiAccessLog(operateName: "删除菜单", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteMenusAsync([FromBody] List<long> ids)
        {
            await _menuService.DeleteMenusAsync(ids);
            return Result.Ok();
        }

        /// <summary>
        /// 获取菜单组成的选项树
        /// </summary>
        /// <returns></returns>
        [HttpGet("MenuOptions")]
        public async Task<AppResponse<Dictionary<string, object>>> GetMenuOptionsAsync(bool onlyMenu, string? keyword)
        {
            var (keys, tree) = await _menuService.GetMenuOptionsAsync(onlyMenu, keyword);
            return Result.Data(new Dictionary<string, object> { ["keys"] = keys, ["tree"] = tree });
        }

        [HttpGet("{id}")]
        [HasPermission("Sys.Menu.List")]
        public async Task<AppResponse<MenuItem>> GetMenuAsync([FromRoute] long id)
        {
            var data = await _menuService.GetMenuAsync(id);
            return Result.Data(data);
        }
    }
}