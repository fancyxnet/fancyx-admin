using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Enums;

namespace Fancyx.DataAccess.Entities.System
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("role")]
    public class Role : FullAuditedEntity, ITenant
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [NotNull]
        [StringLength(64)]
        [Column("role_name")]
        public string? RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public virtual ICollection<RoleMenu>? RoleMenus { get; set; }

        /// <summary>
        /// 角色查看部门（数据权限类型=<see cref="DeptPowerType.Specify"/>时，指定部门时才存入）
        /// </summary>
        public virtual ICollection<RoleDept>? RoleDepts { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// 部门权限类型
        /// </summary>
        [Column("dept_power_type")]
        public DeptPowerType DeptPowerType { get; set; } = DeptPowerType.All;
    }
}