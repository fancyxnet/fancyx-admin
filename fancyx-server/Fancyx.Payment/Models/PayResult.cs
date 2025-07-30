namespace Fancyx.Payment.Models
{
    public class PayResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 付款单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 重定向URL，用于网页支付
        /// </summary>
        public string? RedirectUrl { get; set; }
    }
}