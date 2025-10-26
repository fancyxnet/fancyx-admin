using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("role_menu")]
    public class RoleMenu : Entity<long>, ITenant
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [Column("menu_id")]
        public long MenuId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}