using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;

using FreeSql.DataAnnotations;

namespace Fancyx.Admin.Entities.Organization
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table(Name = "org_position")]
    public class PositionDO : FullAuditedEntity, ITenant
    {
        /// <summary>
        /// 职位编号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column(IsNullable = false, StringLength = 32)]
        public string? Code { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column(IsNullable = false, StringLength = 64)]
        public string? Name { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        [NotNull]
        [Required]
        [Range(1, int.MaxValue)]
        [Column(IsNullable = false)]
        public int Level { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(512)]
        [Column(StringLength = 512)]
        public string? Description { get; set; }

        /// <summary>
        /// 职位分组
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsNullable = true, StringLength = 18)]
        public string? TenantId { get; set; }
    }
}