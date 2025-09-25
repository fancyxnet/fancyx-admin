using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("good_bind_attr_value")]
    public class GoodBindAttrValue
    {
        [Column("good_id")]
        public long GoodId { get; set; }

        [Column("attr_id")]
        public long AttrId { get; set; }

        [Column("attr_value")]
        public string? AttrValue { get; set; }
    }
}