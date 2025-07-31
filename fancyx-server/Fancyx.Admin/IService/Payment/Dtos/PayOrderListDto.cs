using Fancyx.Payment.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.IService.Payment.Dtos
{
    public class PayOrderListDto
    {
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Required]
        public Guid ProviderId { get; set; }

        /// <summary>
        /// 支付类型（支付宝或微信）
        /// </summary>
        [Required]
        public PaymentType Type { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string? PayStatus { get; set; }

        /// <summary>
        /// 发起支付时间
        /// </summary>
        public DateTime InitiationTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 订单金额（单位元）
        /// </summary>
        public decimal OrderAmount { get; set; } = 0;

        /// <summary>
        /// 实际支付金额（单位元）
        /// </summary>
        public decimal RealAmount { get; set; } = 0;

        /// <summary>
        /// 退款金额（单位元）
        /// </summary>
        public decimal RefundAmount { get; set; } = 0;

        /// <summary>
        /// 退款标识号
        /// </summary>
        public string? RefundNo { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
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
