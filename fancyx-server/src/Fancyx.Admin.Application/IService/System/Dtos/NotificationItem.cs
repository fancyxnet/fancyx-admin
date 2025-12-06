namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class NotificationItem
    {
        public long Id { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 通知用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 是否已读(1已读0未读)
        /// </summary>
        public bool IsReaded { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 已读时间
        /// </summary>
        public DateTime? ReadedTime { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? NickName { get; set; }
    }
}