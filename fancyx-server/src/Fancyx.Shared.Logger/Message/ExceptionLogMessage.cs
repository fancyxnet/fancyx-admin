namespace Fancyx.Logger.Message
{
    public class ExceptionLogMessage
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; } = null!;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; } = null!;

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public string StackTrace { get; set; } = null!;

        /// <summary>
        /// 内部异常信息
        /// </summary>
        public string? InnerException { get; set; }

        /// <summary>
        /// 请求路径 (如果是Web请求)
        /// </summary>
        public string? RequestPath { get; set; }

        /// <summary>
        /// 请求方法 (GET, POST等)
        /// </summary>
        public string? RequestMethod { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        public string? TraceId { get; set; }

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
    }
}