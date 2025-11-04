using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    /// <summary>
    /// 产品属性绑定值
    /// </summary>
    [Table("product_bind_attr_value")]
    public class ProductBindAttrValue : CreationEntity<long>, ITenant
    {
        [Column("product_id")]
        public long ProductId { get; set; }

        [Column("attr_id")]
        public long AttrId { get; set; }

        [Column("attr_value")]
        public string? AttrValue { get; set; }

        [Column("attr_value_id")]
        public long? AttrValueId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}