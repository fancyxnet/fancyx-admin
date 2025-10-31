using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class TenantDto
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(64)]
        public string? Name { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(18)]
        public string? TenantId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 租户域名
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string? Domain { get; set; }
    }
}