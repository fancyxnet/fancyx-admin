using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;
using Fancyx.Shared.Consts;
using Fancyx.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using MiniExcelLibs;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.Application.IService.System;

namespace Fancyx.Admin.Controllers.System
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
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
        [HttpPost("Add")]
        [HasPermission("Sys.User.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        [ApiAccessLog(operateName: "新增用户", operateType: [OperateType.Create], reponseEnable: true)]
        public async Task<AppResponse<bool>> AddUserAsync([FromBody] AddUserRequest dto)
        {
            await _userService.AddUserAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Sys.User.List")]
        [ApiAccessLog(operateName: "用户分页列表")]
        public async Task<AppResponse<PagedResult<UserItem>>> GetUserListAsync([FromQuery] GetUserListRequest dto)
        {
            var data = await _userService.GetUserListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id:long}")]
        [HasPermission("Sys.User.Delete")]
        public async Task<AppResponse<bool>> DeleteUserAsync(long id)
        {
            await _userService.DeleteUserAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AssignRole")]
        [HasPermission("Sys.User.AssignRole")]
        [ApiAccessLog(operateName: "分配角色", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> AssignRoleAsync([FromBody] AssignRoleRequest dto)
        {
            await _userService.AssignRoleAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 切换用户启用状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("ChangeEnabled/{id:long}")]
        [HasPermission("Sys.User.SwitchEnabledStatus")]
        [ApiAccessLog(operateName: "切换用户启用状态", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> SwitchUserEnabledStatusAsync(long id)
        {
            await _userService.SwitchUserEnabledStatusAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 获取指定用户角色
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("Roles/{uid:long}")]
        public async Task<AppResponse<long[]>> GetUserRoleIdsAsync(long uid)
        {
            var data = await _userService.GetUserRoleIdsAsync(uid);
            return Result.Data(data);
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("ResetPwd")]
        [HasPermission("Sys.User.ResetPwd")]
        [ApiAccessLog(operateName: "重置用户密码", operateType: [OperateType.Update], reponseEnable: true)]
        public async Task<AppResponse<bool>> ResetUserPasswordAsync([FromBody] ResetUserPwdRequest dto)
        {
            await _userService.ResetUserPasswordAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 用户简单信息查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("SimpleUserInfos")]
        public async Task<AppResponse<List<UserSimpleInfo>>> GetUserSimpleInfosAsync(string? keyword)
        {
            var data = await _userService.GetUserSimpleInfosAsync(keyword);
            return Result.Data(data);
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("Export")]
        [HasPermission("Sys.User.Export")]
        [ApiAccessLog(operateName: "导出用户列表")]
        public async Task<IActionResult> ExportUserListAsync([FromQuery] GetUserListRequest dto)
        {
            var data = await _userService.ExportUserListAsync(dto);
            var memoryStream = new MemoryStream();
            memoryStream.SaveAs(data);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, MimeTypesHelper.Instance.GetMimeTypeByExtension("xlsx"), "用户列表.xlsx");
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Sys.User.Update")]
        [ApiAccessLog(operateName: "修改用户")]
        public async Task<AppResponse<bool>> UpdateUserAsync([FromBody] UpdateUserRequest dto)
        {
            await _userService.UpdateUserAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 用户编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("EditInfo")]
        public async Task<AppResponse<UserDetails>> GetUserEditInfoAsync(long id)
        {
            var data = await _userService.GetUserEditInfoAsync(id);
            return Result.Data(data);
        }
    }
}