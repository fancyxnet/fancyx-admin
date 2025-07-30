namespace Fancyx.Payment.Models
{
    public class TradeQueryResult
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; }

        /// <summary>
        /// 当前支付状态
        /// </summary>
        public string? PayStatus { get; set; }

        #region 支付宝返回字段

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string? TradeNo { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string? OutTradeNo { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string? TradeStatus { get; set; }

        /// <summary>
        /// 交易的订单金额，单位为元，两位小数
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 买家在支付宝的用户id
        /// </summary>
        public string? BuyerUserId { get; set; }

        #endregion 支付宝返回字段
    }
}