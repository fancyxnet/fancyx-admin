using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Erp.EfCore.Entities
{
    /// <summary>
    /// 库存
    /// </summary>
    [Table("inventory")]
    public class Inventory : AuditedEntity<long>, ITenant
    {
        /// <summary>
        /// 库存编号
        /// </summary>
        [NotNull, Required]
        public string? InventoryNo { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Column("product_id")]
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}