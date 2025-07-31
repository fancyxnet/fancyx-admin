namespace Fancyx.Admin.IService.Payment.Dtos
{
    public class PayOrderQueryDto : PageSearch
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 退款标识号
        /// </summary>
        public string? RefundNo { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public Guid? ProviderId { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string? PayStatus { get; set; }
    }
}