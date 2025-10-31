using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.EfCore.Entities.System
{
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
        [Required]
        [Column("domain")]
        public string? Domain { get; set; }
    }
}