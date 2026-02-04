using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Cracker.IdentityServer.Abstractions;
using Cracker.EfCore.BaseEntity;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table("dict_type")]
    public class DictType : AuditedEntity<long>, ITenant
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column("dict_type")]
        public string? Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}