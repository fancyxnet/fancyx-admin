using Fancyx.Admin.IService.Account.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.Account
{
    public interface IAccountService : IScopedDependency
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<LoginResultDto> LoginAsync(LoginDto dto);

        /// <summary>
        /// 短信验证码登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<LoginResultDto> SmsLoginAsync(SmsLoginDto dto);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<TokenResultDto> GetAccessTokenAsync(string refreshToken);

        /// <summary>
        /// 修改个人基本信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateUserInfoAsync(PersonalInfoDto dto);

        /// <summary>
        /// 修改个人密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateUserPwdAsync(UserPwdDto dto);

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        Task<bool> SignOutAsync();

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <returns></returns>
        Task<UserAuthInfoDto> GetUserAuthInfoAsync();

        /// <summary>
        /// 发送登录短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<string> SendLoginSmsCodeAsync(string phone);
    }
}