using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.DataAccess.Entities.Log
{
    /// <summary>
    /// 异常日志实体类
    /// </summary>
    [Table("exception_log")]
    public class ExceptionLogDO : CreationEntity, ITenant
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        [Column("exception_type")]
        public string ExceptionType { get; set; } = null!;

        /// <summary>
        /// 异常消息
        /// </summary>
        [Column("message")]
        public string Message { get; set; } = null!;

        /// <summary>
        /// 异常堆栈
        /// </summary>
        [Column("stack_trace")]
        public string StackTrace { get; set; } = null!;

        /// <summary>
        /// 内部异常信息
        /// </summary>
        [Column("inner_exception")]
        public string? InnerException { get; set; }

        /// <summary>
        /// 请求路径 (如果是Web请求)
        /// </summary>
        [Column("request_path")]
        public string? RequestPath { get; set; }

        /// <summary>
        /// 请求方法 (GET, POST等)
        /// </summary>
        [Column("request_method")]
        public string? RequestMethod { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [StringLength(32)]
        [Column("ip")]
        public string? Ip { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        [Column("browser")]
        public string? Browser { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        [Column("trace_id")]
        public string? TraceId { get; set; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        [Column("is_handled")]
        public bool IsHandled { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [Column("handled_time")]
        public DateTime? HandledTime { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [Column("handled_by")]
        public string? HandledBy { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}