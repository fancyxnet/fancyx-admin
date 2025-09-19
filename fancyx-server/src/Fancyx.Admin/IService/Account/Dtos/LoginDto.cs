using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.Account.Dtos
{
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [NotNull]
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotNull]
        [Required]
        public string? Password { get; set; }
    }
}