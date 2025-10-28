using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using Fancyx.Erp.EfCore.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    public class InventoryLog : CreationEntity<long>, ITenant
    {
        public InventoryBizType BizType { get; set; }
        public long InventoryId { get; set; }
        public string? InventoryNo { get; set; }
        public InventorySource Source { get; set; }
        public string? SoureNo { get; set; }
        public string? Remark { get; set; }
        public int ChangeQuantity { get; set; }
        public int AfterQuantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}