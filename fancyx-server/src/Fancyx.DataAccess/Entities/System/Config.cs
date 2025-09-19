using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;

namespace Fancyx.DataAccess.Entities.System
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Table("config")]
    public class Config : AuditedEntity, ITenant
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [NotNull]
        [Required]
        [Column("name")]
        [StringLength(256)]
        public string? Name { get; set; }

        /// <summary>
        /// 配置键名
        /// </summary>
        [NotNull]
        [Required]
        [Column("key")]
        [StringLength(128)]
        public string? Key { get; set; }

        /// <summary>
        /// 配置键值
        /// </summary>
        [NotNull]
        [Required]
        [Column("value")]
        public string? Value { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [StringLength(64)]
        [Column("group_key")]
        public string? GroupKey { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}