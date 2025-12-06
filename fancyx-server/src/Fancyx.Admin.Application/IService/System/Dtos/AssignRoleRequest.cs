using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignRoleRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long[]? RoleIds { get; set; }
    }
}