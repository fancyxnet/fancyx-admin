using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.EfCore.Entities.System
{
    [Table("tenant")]
    [Index(nameof(TenantId), IsUnique = true)]
    public class Tenant : AuditedEntity<long>
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [NotNull]
        [Required]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(18)]
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 租户域名
        /// </summary>
        [Required]
        [Column("domain")]
        public string? Domain { get; set; }
    }
}