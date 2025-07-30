using System.ComponentModel;

namespace Fancyx.Payment.Enums
{
    public enum PaymentType
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 1,

        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WeChatPay = 2,
    }
}