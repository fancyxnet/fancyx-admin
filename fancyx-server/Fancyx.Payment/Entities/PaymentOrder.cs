using Fancyx.Payment.Enums;
using Fancyx.Repository.BaseEntity;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Fancyx.Payment.Entities
{
    /// <summary>
    /// 付款单
    /// </summary>
    [Table(Name = "pay_order")]
    [Index("uk_payment_order_no", "OrderNo", true)]
    [Index("uk_payment_trade_no", "TradeNo", true)]
    public class PaymentOrder : AuditedEntity
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Required]
        [Column(IsNullable = false)]
        public Guid ProviderId { get; set; }

        /// <summary>
        /// 支付类型（支付宝或微信）
        /// </summary>
        [Required]
        [Column(IsNullable = false)]
        public PaymentType Type { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Column(IsNullable = false, StringLength = 32)]
        public string? OrderNo { get; set; }

        /// <summary>
        /// 支付状态，<see cref="PayStatusExtension.GetStatus(Enums.PayStatus)"/>字符串值
        /// </summary>
        [Column(IsNullable = false, StringLength = 16)]
        public string? PayStatus { get; set; }

        /// <summary>
        /// 发起支付时间
        /// </summary>
        [Column(IsNullable = false)]
        public DateTime InitiationTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column(IsNullable = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 订单金额（单位元）
        /// </summary>
        [Column(IsNullable = false)]
        public decimal OrderAmount { get; set; } = 0;

        /// <summary>
        /// 实际支付金额（单位元）
        /// </summary>
        [Column(IsNullable = false)]
        public decimal RealAmount { get; set; } = 0;

        /// <summary>
        /// 退款金额（单位元）
        /// </summary>
        [Column(IsNullable = false)]
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// 退款标识号
        /// </summary>
        [Column(IsNullable = true, StringLength = 32)]
        public string? RefundNo { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        [Column(IsNullable = true, StringLength = 512)]
        public string? RefundReason { get; set; }

        /// <summary>
        /// 付款描述，例如商品名称或服务描述
        /// </summary>
        public string? PayDesc { get; set; }

        /// <summary>
        /// 支付成功时间
        /// </summary>
        public DateTime? PayedTime { get; set; }

        /// <summary>
        /// 取消支付时间
        /// </summary>
        public DateTime? CancelTime { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public DateTime? TimeoutTime { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundTime { get; set; }

        /// <summary>
        /// 支付宝交易凭证号
        /// </summary>
        public string? TradeNo { get; set; }
    }
}