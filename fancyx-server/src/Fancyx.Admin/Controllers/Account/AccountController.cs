using Fancyx.Admin.IService.Account;
using Fancyx.Admin.IService.Account.Dtos;
using Fancyx.Shared.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<LoginResultDto>> LoginAsync([FromBody] LoginDto dto)
        {
            var data = await _accountService.LoginAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 手机短信登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SmsLogin")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<LoginResultDto>> SmsLoginAsync([FromBody] SmsLoginDto dto)
        {
            var data = await _accountService.SmsLoginAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<AppResponse<TokenResultDto>> GetAccessTokenAsync(string refreshToken)
        {
            var data = await _accountService.GetAccessTokenAsync(refreshToken);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改个人基本信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("updateInfo")]
        public async Task<AppResponse<bool>> UpdateUserInfoAsync([FromBody] PersonalInfoDto dto)
        {
            await _accountService.UpdateUserInfoAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 修改个人密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("updatePwd")]
        public async Task<AppResponse<bool>> UpdateUserPwdAsync([FromBody] UserPwdDto dto)
        {
            await _accountService.UpdateUserPwdAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpPost("signOut")]
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
        [HttpGet("userAuth")]
        public async Task<AppResponse<UserAuthInfoDto>> GetUserAuthInfoAsync()
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