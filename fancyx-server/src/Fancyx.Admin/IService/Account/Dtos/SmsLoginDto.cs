using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.Account.Dtos
{
    public class SmsLoginDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Phone]
        [NotNull]
        [Required]
        [MaxLength(11)]
        public string? Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Phone]
        [NotNull]
        [Required]
        [MaxLength(6)]
        public string? Code { get; set; }
    }
}
