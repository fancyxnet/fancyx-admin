using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;

namespace Fancyx.Admin.EfCore.Entities.Organization
{
    /// <summary>
    /// 职位分组
    /// </summary>
    [Table("position_group")]
    public class PositionGroup : AuditedEntity, ITenant, ITreeEntity
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
        /// 树形路径
        /// </summary>
        [StringLength(1024)]
        [Column("tree_path")]
        [NotNull, Required]
        public string TreePath { get; set; } = null!;

        /// <summary>
        /// 树形层级
        /// </summary>
        [DefaultValue(0)]
        [Column("tree_level")]
        public int TreeLevel { get; set; }

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
    }
}