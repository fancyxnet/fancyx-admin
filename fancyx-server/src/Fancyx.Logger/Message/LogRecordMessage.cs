namespace Fancyx.Logger.Message
{
    public class LogRecordMessage
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string? Type { get; set; } = null!;

        /// <summary>
        /// 日志子类型
        /// </summary>
        public string? SubType { get; set; } = null!;

        /// <summary>
        /// 业务编号/ID
        /// </summary>
        public string? BizNo { get; set; } = null!;

        /// <summary>
        /// 操作内容
        /// </summary>
        public string? Content { get; set; } = null!;

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 脱敏字段
        /// </summary>
        public string[]? SanitizeKeys { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// 用户请求头
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public string? TenantId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}