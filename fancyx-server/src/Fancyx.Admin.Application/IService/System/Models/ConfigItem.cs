using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Models
{
    public class ConfigItem
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Key { get; set; } = null!;

        [Required]
        public string? Value { get; set; } = null!;

        public string? GroupKey { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }
}
