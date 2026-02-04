using Cracker.IdentityServer.Abstractions;
using Cracker.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Erp.EfCore.Entities
{
    /// <summary>
    /// 产品
    /// </summary>
    [Table("product")]
    public class Product : AuditedEntity<long>, ITenant
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        [Column("code")]
        public string Code { get; set; } = null!;

        /// <summary>
        /// SKU编号
        /// </summary>
        [Column("sku_code")]
        public string SkuCode { get; set; } = null!;

        /// <summary>
        /// 产品名称
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
        /// 品牌ID
        /// </summary>
        [Column("brand_id")]
        public long BrandId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [Column("category_id")]
        public long CategoryId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 单位,取字典
        /// </summary>
        [NotNull, Required]
        public string? Unit { get; set; }
    }
}