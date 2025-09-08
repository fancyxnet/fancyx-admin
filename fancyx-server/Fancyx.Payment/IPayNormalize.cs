using Fancyx.DataAccess.Enums;
using Fancyx.Payment.Models;

namespace Fancyx.Payment
{
    public interface IPayNormalize
    {
        PaymentType PayType { get; }

        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PayResult> CreateOrderAsync(CreateOrderRequest req);

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Task<TradeQueryResult> QueryTradeStatusAsync(string orderNo);

        /// <summary>
        /// 处理支付回调通知
        /// </summary>
        /// <param name="notifyMap"></param>
        /// <returns></returns>
        Task<PayCallBackResult> CallBackAsync(Dictionary<string, string> notifyMap);

        /// <summary>
        /// 发起退款
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<RefundResult> RefundAsync(RefundRequest req);
    }
}