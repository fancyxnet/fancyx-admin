namespace Fancyx.Payment.Enums
{
    public enum PayStatus
    {
        /// <summary>
        /// 支付成功
        /// </summary>
        Success,

        /// <summary>
        /// 支付失败
        /// </summary>
        Failed,

        /// <summary>
        /// 支付中
        /// </summary>
        Processing,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled,

        /// <summary>
        /// 超时
        /// </summary>
        Timeout,

        /// <summary>
        /// 已退款
        /// </summary>
        Refunded
    }

    public static class PayStatusExtension
    {
        public static string GetStatus(this PayStatus payStatus)
        {
            return payStatus switch
            {
                PayStatus.Success => "success",
                PayStatus.Failed => "failed",
                PayStatus.Processing => "processing",
                PayStatus.Canceled => "canceled",
                PayStatus.Timeout => "timeout",
                PayStatus.Refunded => "refunded",
                _ => throw new NotSupportedException()
            };
        }
    }
}