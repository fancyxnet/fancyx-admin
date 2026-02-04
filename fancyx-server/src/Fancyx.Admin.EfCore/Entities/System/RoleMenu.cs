using System.ComponentModel.DataAnnotations.Schema;

using Cracker.IdentityServer.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Table("role_menu")]
    [PrimaryKey(nameof(MenuId), nameof(RoleId))]
    public class RoleMenu : ITenant
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