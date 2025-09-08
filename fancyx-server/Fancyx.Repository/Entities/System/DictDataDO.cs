using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;


namespace Fancyx.Repository.Entities.System
{
    /// <summary>
    /// 字典数据表
    /// </summary>
    [Table("sys_dict_data")]
    public class DictDataDO : AuditedEntity, ITenant
    {
        /// <summary>
        /// 字典值
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(256)]
        [Column("value")]
        public string? Value { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column("label")]
        public string? Label { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column("dict_type")]
        public string? DictType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}