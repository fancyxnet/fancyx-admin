using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Table("inventory")]
    public class Inventory : AuditedEntity
    {
        [Column("product_id")]
        public long ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}