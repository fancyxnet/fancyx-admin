namespace Fancyx.Admin.Application.IService.Account.Models
{
    public class LoginRespone : TokenResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string? SessionId { get; set; }
    }
}