using Fancyx.Shared.Consts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class UserDto : UserEditDto
    {
        /// <summary>
        /// 用户名（大小写字母，数字，下划线，长度3-12位）
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32, MinimumLength = 3)]
        [RegularExpression(RegexConsts.UserName)]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotNull]
        [Required]
        [MinLength(6)]
        [RegularExpression(RegexConsts.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(256)]
        public string? Avatar { get; set; }
    }
}