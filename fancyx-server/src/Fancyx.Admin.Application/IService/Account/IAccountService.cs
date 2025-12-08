using Fancyx.Admin.Application.IService.Account.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Account
{
    public interface IAccountService : IScopedDependency
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<LoginRespone> LoginAsync(LoginRequest req);

        /// <summary>
        /// 短信验证码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<LoginRespone> SmsLoginAsync(SmsLoginRequest req);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<TokenResponse> GetAccessTokenAsync(string refreshToken);

        /// <summary>
        /// 修改个人基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateUserInfoAsync(UpdateUserInfoRequest req);

        /// <summary>
        /// 修改个人密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateUserPwdAsync(UpdateUserPwdRequest req);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task<bool> SignOutAsync();

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <returns></returns>
        Task<GetUserAuthInfoResponse> GetUserAuthInfoAsync();

        /// <summary>
        /// 发送登录短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<string> SendLoginSmsCodeAsync(string phone);
    }
}