namespace Fancyx.Admin.Application.IService.Organization.Models
{
    public class GetDeptListRequest : PageSearch
    {
        /// <summary>
        /// 部门名称/编号
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        public int Status { get; set; }
    }
}