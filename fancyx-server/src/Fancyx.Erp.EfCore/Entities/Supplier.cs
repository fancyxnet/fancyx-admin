using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entities
{
    /// <summary>
    /// 供应商
    /// </summary>
    [Table("supplier")]
    public class Supplier : AuditedEntity<long>, ITenant
    {
        /// <summary>
        /// 编码
        /// </summary>
        [Column("code")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 是否启用
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