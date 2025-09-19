namespace Fancyx.Admin.IService.System.LogManagement.Dtos
{
    public class BusinessLogQueryDto : PageSearch
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 业务子类型
        /// </summary>
        public string? SubType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string? UserName { get; set; }
    }
}