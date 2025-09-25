using Fancyx.Admin.EfCore.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class UserEditDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(64)]
        public string? NickName { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid? DeptId { get; set; }

        /// <summary>
        /// 岗位ID
        /// </summary>
        public Guid? PostId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [NotNull]
        [Required]
        [Range(1, 3)]
        public SexType Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Phone]
        public string? Phone { get; set; }
    }
}
