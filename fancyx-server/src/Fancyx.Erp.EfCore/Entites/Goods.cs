using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("goods")]
    [Index(nameof(Code), IsUnique = true)]
    [Index(nameof(SkuCode), IsUnique = true)]
    public class Goods : FullAuditedEntity
    {
        [Column("code")]
        public string Code { get; set; } = null!;
        [Column("sku_code")]
        public string SkuCode { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("remark")]
        public string? Remark { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("brand_id")]
        public long BrandId { get; set; }

        [Column("classify_id")]
        public long ClassifyId { get; set; }
    }
}