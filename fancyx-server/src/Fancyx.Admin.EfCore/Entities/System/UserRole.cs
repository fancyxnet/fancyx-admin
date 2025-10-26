using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table("user_role")]
    public class UserRole : Entity<long>, ITenant
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