using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Repository.Entities.Organization
{
    /// <summary>
    /// 职位分组
    /// </summary>
    [Table("org_position_group")]
    public class PositionGroupDO : AuditedEntity, ITenant
    {
        /// <summary>
        /// 分组名
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("group_name")]
        public string? GroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 层级父ID
        /// </summary>
        [StringLength(1024)]
        [Column("parent_ids")]
        public string? ParentIds { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [Required]
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        public PositionGroupDO? Parent { get; set; }

        public List<PositionGroupDO>? Children { get; set; }
    }
}