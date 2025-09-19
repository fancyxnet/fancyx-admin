using Fancyx.Shared.Consts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.Account.Dtos
{
    public class UserPwdDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [NotNull]
        [Required]
        public string? OldPwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [NotNull]
        [Required]
        [MinLength(6)]
        [RegularExpression(RegexConsts.Password)]
        public string? NewPwd { get; set; }
    }
}