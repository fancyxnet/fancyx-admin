namespace Fancyx.Payment.Models
{
    public class CreateOrderRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// 总付款金额
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}