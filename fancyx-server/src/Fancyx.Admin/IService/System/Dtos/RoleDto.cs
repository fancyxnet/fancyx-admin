using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(64)]
        public string? RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [NotNull]
        [Required]
        public bool IsEnabled { get; set; }
    }
}