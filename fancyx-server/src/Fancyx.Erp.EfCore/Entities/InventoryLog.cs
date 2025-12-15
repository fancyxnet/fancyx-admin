using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using Fancyx.Erp.EfCore.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entities
{
    /// <summary>
    /// 库存日志
    /// </summary>
    public class InventoryLog : CreationEntity<long>, ITenant
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public InventoryBizType BizType { get; set; }

        /// <summary>
        /// 库存ID
        /// </summary>
        public long InventoryId { get; set; }

        /// <summary>
        /// 库存编号
        /// </summary>
        public string? InventoryNo { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public InventorySource Source { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string? SoureNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 改变数量
        /// </summary>
        public int ChangeQuantity { get; set; }

        /// <summary>
        /// 改变后数量
        /// </summary>
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