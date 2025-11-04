using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    /// <summary>
    /// 产品属性可选值
    /// </summary>
    [Table("product_attr_value")]
    public class ProductAttrValue : FullAuditedEntity<long>, ITenant
    {
        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("code")]
        public string Value { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("remark")]
        public string? Remark { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("attr_id")]
        public long AttrId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}