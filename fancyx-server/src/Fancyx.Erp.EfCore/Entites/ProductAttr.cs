using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("product_attr")]
    [Index(nameof(Code), IsUnique = true)]
    public class ProductAttr : FullAuditedEntity<long>
    {
        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("remark")]
        public string? Remark { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

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

        public long? CategoryId { get; set; }
    }
}