namespace Fancyx.Admin.IService.Account.Dtos
{
    public class UserNotificationNavbarDto
    {
        public int NoReadedCount { get; set; }

        public List<UserNotificationNavbarItemDto>? Items { get; set; }
    }

    public class UserNotificationNavbarItemDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 是否已读(1已读0未读)
        /// </summary>
        public bool IsReaded { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
