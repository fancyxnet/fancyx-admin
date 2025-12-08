namespace Fancyx.Erp.Application.IService.BaseInfo.Models
{
    public class AddOrUpdateCustomerRequest
    {
        public virtual long? Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// 简码
        /// </summary>
        public string? CodeSlim { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string? ContactPhone { get; set; }
    }
}
