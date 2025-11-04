using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Erp.EfCore.Entites
{
    /// <summary>
    /// 库存
    /// </summary>
    [Table("inventory")]
    public class Inventory : AuditedEntity<long>, ITenant
    {
        [NotNull, Required]
        public string? InventoryNo { get; set; }

        [Column("product_id")]
        public long ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public long WarehouseId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}