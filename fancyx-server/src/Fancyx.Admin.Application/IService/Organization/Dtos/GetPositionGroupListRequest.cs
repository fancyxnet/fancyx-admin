namespace Fancyx.Admin.Application.IService.Organization.Dtos
{
    public class GetPositionGroupListRequest : PageSearch
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public string? GroupName { get; set; }
    }
}