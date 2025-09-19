using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class AssignRoleDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid[]? RoleIds { get; set; }
    }
}