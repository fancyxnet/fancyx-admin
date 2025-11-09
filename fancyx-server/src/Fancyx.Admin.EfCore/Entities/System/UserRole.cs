using System.ComponentModel.DataAnnotations.Schema;

using Fancyx.Core.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table("user_role")]
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole : ITenant
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public long UserId { get; set; }

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