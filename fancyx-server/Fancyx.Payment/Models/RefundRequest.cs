namespace Fancyx.Payment.Models
{
    public class RefundRequest
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 退款金额（单位元）
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string? Reason { get; set; }
    }
}