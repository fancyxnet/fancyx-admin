using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;

using FreeSql.DataAnnotations;

namespace Fancyx.Admin.Entities.Organization
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table(Name = "sys_dept")]
    public class DeptDO : FullAuditedEntity, ITenant
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column(IsNullable = false, StringLength = 32)]
        public string? Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column(IsNullable = false, StringLength = 64)]
        public string? Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(512)]
        [Column(StringLength = 512)]
        public string? Description { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? CuratorId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(64)]
        [Column(StringLength = 64)]
        public string? Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(64)]
        [Column(StringLength = 64)]
        public string? Phone { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 层级父ID
        /// </summary>
        [StringLength(1024)]
        [Column(StringLength = 1024)]
        public string? ParentIds { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [DefaultValue(0)]
        [Column(IsNullable = false)]
        public int Layer { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsNullable = true, StringLength = 18)]
        public string? TenantId { get; set; }

        [Navigate(nameof(ParentId))]
        public DeptDO? Parent { get; set; }

        [Navigate(nameof(ParentId))]
        public List<DeptDO>? Children { get; set; }
    }
}