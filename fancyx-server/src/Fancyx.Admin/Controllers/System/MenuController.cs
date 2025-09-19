using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("api/menu")]
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Sys.Menu.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddMenuAsync([FromBody] MenuDto dto)
        {
            await _menuService.AddMenuAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 菜单树形列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Sys.Menu.List")]
        [ApiAccessLog(operateType: [OperateType.Query])]
        public async Task<AppResponse<List<MenuListDto>>> GetMenuListAsync([FromQuery] MenuQueryDto dto)
        {
            var data = await _menuService.GetMenuListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [HasPermission("Sys.Menu.Update")]
        [ApiAccessLog(operateName: "修改菜单", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> UpdateMenuAsync([FromBody] MenuDto dto)
        {
            await _menuService.UpdateMenuAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [HasPermission("Sys.Menu.Delete")]
        [ApiAccessLog(operateName: "删除菜单", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteMenusAsync([FromBody] Guid[] ids)
        {
            await _menuService.DeleteMenusAsync(ids);
            return Result.Ok();
        }

        /// <summary>
        /// 获取菜单组成的选项树
        /// </summary>
        /// <returns></returns>
        [HttpGet("menuOptions")]
        public async Task<AppResponse<Dictionary<string, object>>> GetMenuOptionsAsync(bool onlyMenu, string? keyword)
        {
            var (keys, tree) = await _menuService.GetMenuOptionsAsync(onlyMenu, keyword);
            return Result.Data(new Dictionary<string, object> { ["keys"] = keys, ["tree"] = tree });
        }
    }
}