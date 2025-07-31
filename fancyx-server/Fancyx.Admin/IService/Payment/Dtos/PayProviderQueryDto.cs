namespace Fancyx.Admin.IService.Payment.Dtos
{
    public class PayProviderQueryDto : PageSearch
    {
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}