using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.Account.Models
{
    public class LoginRequest
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