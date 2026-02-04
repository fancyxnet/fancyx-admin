using Cracker.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 租户
    /// </summary>
    [Table("tenant")]
    public class Tenant : AuditedEntity<string>
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [NotNull]
        [Required]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 租户域名
        /// </summary>
        [Required, NotNull]
        [Column("domain")]
        public string? Domain { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
    }
}