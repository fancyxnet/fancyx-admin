using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    /// <summary>
    /// 产品属性
    /// </summary>
    [Table("product_attr")]
    public class ProductAttr : AuditedEntity<long>, ITenant
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
        /// 是否必填
        /// </summary>
        [Column("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// 多选/单选/手动录入
        /// </summary>
        [Column("input_type")]
        public string InputType { get; set; } = null!;

        /// <summary>
        /// 指定分类/所有分类
        /// </summary>
        [Column("attr_type")]
        public int AttrType { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}