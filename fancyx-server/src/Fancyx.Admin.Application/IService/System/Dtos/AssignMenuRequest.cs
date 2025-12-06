using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignMenuRequest
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long[]? MenuIds { get; set; }
    }
}