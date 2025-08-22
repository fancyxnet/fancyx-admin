using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;

using FreeSql.DataAnnotations;

namespace Fancyx.Admin.Entities.System
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table(Name = "sys_role")]
    public class RoleDO : FullAuditedEntity, ITenant
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [NotNull]
        [StringLength(64)]
        [Column(IsNullable = false, StringLength = 64)]
        public string? RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [Column(StringLength = 512)]
        public string? Remark { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<UserRoleDO>? UserRoles { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public virtual ICollection<RoleMenuDO>? RoleMenus { get; set; }

        /// <summary>
        /// 角色查看部门（数据权限类型=1时，指定部门时才存入）
        /// </summary>
        public virtual ICollection<RoleDeptDO>? RoleDepts { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsNullable = true, StringLength = 18)]
        public string? TenantId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column(IsNullable = false)]
        public bool IsEnabled { get; set; } = false;
    }
}