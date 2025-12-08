using Fancyx.Admin.Application.IService.Account;
using Fancyx.Admin.Application.IService.Account.Models;
using Fancyx.Shared.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<LoginRespone>> LoginAsync([FromBody] LoginRequest req)
        {
            var data = await _accountService.LoginAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 手机短信登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SmsLogin")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<LoginRespone>> SmsLoginAsync([FromBody] SmsLoginRequest req)
        {
            var data = await _accountService.SmsLoginAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        public async Task<AppResponse<TokenResponse>> GetAccessTokenAsync(string refreshToken)
        {
            var data = await _accountService.GetAccessTokenAsync(refreshToken);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改个人基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("UpdateInfo")]
        public async Task<AppResponse<bool>> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest req)
        {
            await _accountService.UpdateUserInfoAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 修改个人密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("UpdatePwd")]
        public async Task<AppResponse<bool>> UpdateUserPwdAsync([FromBody] UpdateUserPwdRequest req)
        {
            await _accountService.UpdateUserPwdAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpPost("SignOut")]
        [AllowAnonymous]
        public async Task<AppResponse<bool>> SignOutAsync()
        {
            await _accountService.SignOutAsync();
            return Result.Ok();
        }

        /// <summary>
        /// 用户权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserAuth")]
        public async Task<AppResponse<GetUserAuthInfoResponse>> GetUserAuthInfoAsync()
        {
            var data = await _accountService.GetUserAuthInfoAsync();
            return Result.Data(data);
        }

        /// <summary>
        /// 发送登录短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost("SendLoginSmsCode")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<string>> SendLoginSmsCodeAsync(string phone)
        {
            //TODO: 正式环境不需要将验证码返回给前端
            var code = await _accountService.SendLoginSmsCodeAsync(phone);
            return Result.Data(code);
        }
    }
}