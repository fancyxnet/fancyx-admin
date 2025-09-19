namespace Fancyx.Admin.IService.System.Dtos
{
    public class RoleQueryDto : PageSearch
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string? RoleName { get; set; }
    }
}