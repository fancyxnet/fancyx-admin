using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;

using FreeSql.DataAnnotations;

namespace Fancyx.Admin.Entities.System
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Table(Name = "sys_config")]
    public class ConfigDO : AuditedEntity, ITenant
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [NotNull]
        [Required]
        [Column(IsNullable = false)]
        [StringLength(256)]
        public string? Name { get; set; }

        /// <summary>
        /// 配置键名
        /// </summary>
        [NotNull]
        [Required]
        [Column(IsNullable = false)]
        [StringLength(128)]
        public string? Key { get; set; }

        /// <summary>
        /// 配置键值
        /// </summary>
        [NotNull]
        [Required]
        [Column(IsNullable = false, StringLength = 1024)]
        public string? Value { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [StringLength(64)]
        public string? GroupKey { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsNullable = true, StringLength = 18)]
        public string? TenantId { get; set; }
    }
}