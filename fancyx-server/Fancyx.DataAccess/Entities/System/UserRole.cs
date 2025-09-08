using Fancyx.Core.Interfaces;
using Fancyx.DataAccess.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.DataAccess.Entities.System
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table("sys_user_role")]
    public class UserRole : Entity, ITenant
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public Guid UserId { get; set; }

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