namespace Fancyx.Admin.Application.IService.Monitor.Dtos
{
    public class OnlineUserItem
    {
        public string? UserId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// 登录地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string? Browser { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string? SessionId { get; set; }
    }
}