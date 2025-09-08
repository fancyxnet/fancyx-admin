using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Admin.Entities.System
{
    [Table("sys_role_dept")]
    public class RoleDeptDO : Entity, ITenant
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("role_id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Column("dept_id")]
        public Guid DeptId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}