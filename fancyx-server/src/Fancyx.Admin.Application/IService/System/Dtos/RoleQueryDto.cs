namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class RoleQueryDto : PageSearch
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string? RoleName { get; set; }
    }
}