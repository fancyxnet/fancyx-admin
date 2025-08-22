using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;

using FreeSql.DataAnnotations;

namespace Fancyx.Admin.Entities.System
{
    /// <summary>
    /// 字典数据表
    /// </summary>
    [Table(Name = "sys_dict_data")]
    public class DictDataDO : AuditedEntity, ITenant
    {
        /// <summary>
        /// 字典值
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(256)]
        [Column(IsNullable = false, StringLength = 256)]
        public string? Value { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column(IsNullable = false, StringLength = 128)]
        public string? Label { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column(IsNullable = false, StringLength = 128)]
        public string? DictType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column(StringLength = 512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [Column(IsNullable = false)]
        public int Sort { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsNullable = true, StringLength = 18)]
        public string? TenantId { get; set; }
    }
}