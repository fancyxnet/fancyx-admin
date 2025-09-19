using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("role_menu")]
    public class RoleMenu : Entity, ITenant
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [Column("menu_id")]
        public Guid MenuId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("role_id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}