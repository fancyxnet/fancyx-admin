using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("good_bind_attr_value")]
    public class GoodBindAttrValue
    {
        [Column("good_id")]
        public Guid GoodId { get; set; }

        [Column("attr_id")]
        public Guid AttrId { get; set; }

        [Column("attr_value")]
        public string? AttrValue { get; set; }
    }
}