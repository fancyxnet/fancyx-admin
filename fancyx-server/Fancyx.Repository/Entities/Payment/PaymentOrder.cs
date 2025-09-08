using Fancyx.Payment.Enums;
using Fancyx.Repository.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Repository.Entities.Payment
{
    /// <summary>
    /// 付款单
    /// </summary>
    [Table("pay_order")]
    [Index(nameof(OrderNo), IsUnique = true)]
    [Index(nameof(TradeNo), IsUnique = true)]
    public class PaymentOrder : AuditedEntity
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Required]
        [Column("provider_id")]
        public Guid ProviderId { get; set; }

        /// <summary>
        /// 支付类型（支付宝或微信）
        /// </summary>
        [Required]
        [Column("type")]
        public PaymentType Type { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Column("order_no")]
        public string? OrderNo { get; set; }

        /// <summary>
        /// 支付状态，<see cref="PayStatusExtension.GetStatus(Enums.PayStatus)"/>字符串值
        /// </summary>
        [Column("pay_status")]
        public string? PayStatus { get; set; }

        /// <summary>
        /// 发起支付时间
        /// </summary>
        [Column("initiation_time")]
        public DateTime InitiationTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 订单金额（单位元）
        /// </summary>
        [Column("order_amount")]
        public decimal OrderAmount { get; set; } = 0;

        /// <summary>
        /// 实际支付金额（单位元）
        /// </summary>
        [Column("real_amount")]
        public decimal RealAmount { get; set; } = 0;

        /// <summary>
        /// 退款金额（单位元）
        /// </summary>
        [Column("refund_amount")]
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// 退款标识号
        /// </summary>
        [Column("refund_no")]
        public string? RefundNo { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [Column("refund_reason")]
        public string? RefundReason { get; set; }

        /// <summary>
        /// 付款描述，例如商品名称或服务描述
        /// </summary>
        [Column("pay_desc")]
        public string? PayDesc { get; set; }

        /// <summary>
        /// 支付成功时间
        /// </summary>
        [Column("payed_time")]
        public DateTime? PayedTime { get; set; }

        /// <summary>
        /// 取消支付时间
        /// </summary>
        [Column("cancel_time")]
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        [Column("timeout_time")]
        public DateTime? TimeoutTime { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        [Column("refund_time")]
        public DateTime? RefundTime { get; set; }

        /// <summary>
        /// 支付宝交易凭证号
        /// </summary>
        [Column("trade_no")]
        public string? TradeNo { get; set; }
    }
}