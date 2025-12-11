namespace Fancyx.Admin.Application.IService.System.Models
{
    public class TenantDetails : TenantItem
    {
        /// <summary>
        /// 租户拥有菜单ID
        /// </summary>
        public List<long>? MenuIds { get; set; }
    }
}
