using Cracker.EfCore.BaseEntity;
using Cracker.IdentityServer.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Shared.Logger.Entities
{
    /// <summary>
    /// API访问日志
    /// </summary>
    [Table("api_access_log")]
    public class ApiAccessLog : CreationEntity<long>, ITenant
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        [Column("path")]
        public string Path { get; set; } = null!;

        /// <summary>
        /// HTTP方法 (GET, POST, PUT等)
        /// </summary>
        [Column("method")]
        public string Method { get; set; } = null!;

        /// <summary>
        /// IP
        /// </summary>
        [StringLength(32)]
        [Column("ip")]
        public string? Ip { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        [Column("request_time")]
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        [Column("response_time")]
        public DateTime? ResponseTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        [Column("duration")]
        public long? Duration { get; set; }

        /// <summary>
        /// 用户ID (可为空，未登录用户)
        /// </summary>
        [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>
        [Column("request_body")]
        public string? RequestBody { get; set; }

        /// <summary>
        /// 响应体
        /// </summary>
        [Column("response_body")]
        public string? ResponseBody { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        [Column("browser")]
        public string? Browser { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Column("query_string")]
        public string? QueryString { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        [Column("trace_id")]
        public string? TraceId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Column("operate_type")]
        public OperateType[]? OperateType { get; init; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [Column("operate_name")]
        public string? OperateName { get; init; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}