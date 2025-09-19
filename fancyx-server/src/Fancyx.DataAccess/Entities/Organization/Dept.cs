using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;

namespace Fancyx.Admin.EfCore.Entities.Organization
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table("dept")]
    public class Dept : FullAuditedEntity, ITenant, ITreeEntity
    {
        [Key]
        [NotNull]
        [Required]
        [Column("id")]
        [DataPower(DataPower.DeptId)]
        public override Guid Id { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(512)]
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Column("curator_id")]
        public Guid? CuratorId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(64)]
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(64)]
        [Column("phone")]
        public string? Phone { get; set; }

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
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}