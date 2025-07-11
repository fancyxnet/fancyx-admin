using Fancyx.Repository.BaseEntity;
using Fancyx.Core.Interfaces;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Entities.System
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table(Name = "sys_dict_type")]
    public class DictTypeDO : AuditedEntity, ITenant
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column(IsNullable = false)]
        public string? Name { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(128)]
        [Column(IsNullable = false)]
        public string? DictType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        public string? Remark { get; set; }

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