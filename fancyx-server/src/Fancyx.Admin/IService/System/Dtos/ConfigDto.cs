using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class ConfigDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(256)]
        public string? Name { get; set; }

        /// <summary>
        /// 配置键名
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(128)]
        public string? Key { get; set; } = null!;

        /// <summary>
        /// 配置键值
        /// </summary>
        [Required]
        [MaxLength(1024)]
        public string? Value { get; set; } = null!;

        /// <summary>
        /// 组别
        /// </summary>
        [MaxLength(64)]
        public string? GroupKey { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        public string? Remark { get; set; }
    }
}