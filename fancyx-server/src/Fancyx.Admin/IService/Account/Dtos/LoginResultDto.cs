namespace Fancyx.Admin.IService.Account.Dtos
{
    public class LoginResultDto : TokenResultDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

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