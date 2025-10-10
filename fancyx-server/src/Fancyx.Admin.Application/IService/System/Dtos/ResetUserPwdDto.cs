using Fancyx.Shared.Consts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class ResetUserPwdDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotNull]
        [Required]
        [MinLength(6)]
        [RegularExpression(RegexConsts.Password)]
        public string? Password { get; set; } = null!;
    }
}