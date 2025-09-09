using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;

namespace Fancyx.DataAccess.Entities.System
{
    [Table("sys_notification")]
    public class Notification : AuditedEntity, ITenant
    {
        /// <summary>
        /// 通知标题
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(128)]
        [Column("title")]
        public string? Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        [MaxLength(512)]
        [Column("content")]
        public string? Content { get; set; }

        /// <summary>
        /// 通知员工
        /// </summary>
        [NotNull]
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 是否已读(true已读false未读)
        /// </summary>
        [Required]
        [Column("is_readed")]
        public bool IsReaded { get; set; }

        /// <summary>
        /// 已读时间
        /// </summary>
        [Column("readed_time")]
        public DateTime? ReadedTime { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}