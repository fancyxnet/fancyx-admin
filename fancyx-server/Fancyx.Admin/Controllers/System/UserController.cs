using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Core.Helpers;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using MiniExcelLibs;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Sys.User.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        [ApiAccessLog(operateName: "新增用户", operateType: [OperateType.Create], reponseEnable: true)]
        public async Task<AppResponse<bool>> AddUserAsync([FromBody] UserDto dto)
        {
            await _userService.AddUserAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Sys.User.List")]
        [ApiAccessLog(operateName: "用户分页列表")]
        public async Task<AppResponse<PagedResult<UserListDto>>> GetUserListAsync([FromQuery] UserQueryDto dto)
        {
            var data = await _userService.GetUserListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:Guid}")]
        [HasPermission("Sys.User.Delete")]
        public async Task<AppResponse<bool>> DeleteUserAsync(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("assignRole")]
        [HasPermission("Sys.User.AssignRole")]
        [ApiAccessLog(operateName: "分配角色", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> AssignRoleAsync([FromBody] AssignRoleDto dto)
        {
            await _userService.AssignRoleAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 切换用户启用状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("changeEnabled/{id:Guid}")]
        [HasPermission("Sys.User.SwitchEnabledStatus")]
        [ApiAccessLog(operateName: "切换用户启用状态", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> SwitchUserEnabledStatusAsync(Guid id)
        {
            await _userService.SwitchUserEnabledStatusAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 获取指定用户角色
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("roles/{uid:Guid}")]
        public async Task<AppResponse<Guid[]>> GetUserRoleIdsAsync(Guid uid)
        {
            var data = await _userService.GetUserRoleIdsAsync(uid);
            return Result.Data(data);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("resetPwd")]
        [HasPermission("Sys.User.ResetPwd")]
        [ApiAccessLog(operateName: "重置用户密码", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> ResetUserPasswordAsync([FromBody] ResetUserPwdDto dto)
        {
            await _userService.ResetUserPasswordAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 用户简单信息查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("simpleUserInfos")]
        public async Task<AppResponse<List<UserSimpleInfoDto>>> GetUserSimpleInfosAsync(string? keyword)
        {
            var data = await _userService.GetUserSimpleInfosAsync(keyword);
            return Result.Data(data);
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("export")]
        [HasPermission("Sys.User.Export")]
        [ApiAccessLog(operateName: "导出用户列表")]
        public async Task<IActionResult> ExportUserListAsync([FromQuery] UserQueryDto dto)
        {
            var data = await _userService.ExportUserListAsync(dto);
            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(data);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, MimeTypesHelper.Instance.GetMimeTypeByExtension("xlsx"), "用户列表.xlsx");
        }
    }
}