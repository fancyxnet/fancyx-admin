using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entities
{
    /// <summary>
    /// 产品属性绑定值
    /// </summary>
    [Table("product_bind_attr_value")]
    public class ProductBindAttrValue : CreationEntity<long>, ITenant
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Column("product_id")]
        public long ProductId { get; set; }

        /// <summary>
        /// 属性ID
        /// </summary>
        [Column("attr_id")]
        public long AttrId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [Column("attr_value")]
        public string? AttrValue { get; set; }

        /// <summary>
        /// 属性值ID
        /// </summary>
        [Column("attr_value_id")]
        public long? AttrValueId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}