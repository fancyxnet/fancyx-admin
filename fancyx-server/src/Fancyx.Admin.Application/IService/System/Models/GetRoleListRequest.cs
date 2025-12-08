namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GetRoleListRequest : PageSearch
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string? RoleName { get; set; }
    }
}