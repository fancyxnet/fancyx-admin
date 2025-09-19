using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class DictDataDto
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(256)]
        public string? Value { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(128)]
        public string? Label { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string? DictType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }
    }
}