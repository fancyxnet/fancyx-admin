namespace Fancyx.Payment.Models
{
    /// <summary>
    /// 支付响应基类
    /// </summary>
    public abstract class PayResponseBase
    {
        /// <summary>
        /// 网关返回码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 网关返回码描述
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 业务返回码
        /// </summary>
        public string? SubCode { get; set; }

        /// <summary>
        /// 业务返回码描述
        /// </summary>
        public string? SubMsg { get; set; }
    }
}