using Fancyx.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fancyx.DataAccess.BaseEntity;

namespace Fancyx.DataAccess.Entities.Log
{
    [Table("log_record")]
    public class LogRecordDO : CreationEntity, ITenant
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        [Column("type")]
        public string? Type { get; set; } = null!;

        /// <summary>
        /// 日志子类型
        /// </summary>
        [Column("sub_type")]
        public string? SubType { get; set; } = null!;

        /// <summary>
        /// 业务编号/ID
        /// </summary>
        [Column("biz_no")]
        public string? BizNo { get; set; } = null!;

        /// <summary>
        /// 操作内容
        /// </summary>
        [Column("content")]
        public string? Content { get; set; } = null!;

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        [Column("browser")]
        public string? Browser { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [StringLength(32)]
        [Column("ip")]
        public string? Ip { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        [Column("trace_id")]
        public string? TraceId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Column("user_name")]
        public string? UserName { get; set; }
    }
}