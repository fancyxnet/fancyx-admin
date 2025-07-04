using Fancyx.Shared.Consts;
using Fancyx.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class UserDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 用户名（大小写字母，数字，下划线，长度3-12位）
        /// </summary>
        [NotNull]
        [Required]
        [MinLength(3)]
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
        public string? Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [NotNull]
        [Required]
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [NotNull]
        [Required]
        [Range(1, 3)]
        public SexType Sex { get; set; }
    }
}