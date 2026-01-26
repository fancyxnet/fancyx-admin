namespace Fancyx.Shared.Models
{
    public class UserPermission
    {
        public long UserId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string[]? Roles { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string[]? Auths { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public List<long>? RoleIds { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public List<long>? MenuIds { get; set; }
    }
}