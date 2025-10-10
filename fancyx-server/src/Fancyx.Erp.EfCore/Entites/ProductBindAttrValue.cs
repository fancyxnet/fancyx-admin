using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("product_bind_attr_value")]
    public class ProductBindAttrValue
    {
        [Column("product_id")]
        public long ProductId { get; set; }

        [Column("attr_id")]
        public long AttrId { get; set; }

        [Column("attr_value")]
        public string? AttrValue { get; set; }
    }
}