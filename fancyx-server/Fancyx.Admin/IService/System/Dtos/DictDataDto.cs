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
        public string? Value { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [Required]
        public string? DictType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
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