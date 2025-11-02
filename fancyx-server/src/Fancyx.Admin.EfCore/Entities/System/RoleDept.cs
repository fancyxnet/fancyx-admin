using System.ComponentModel.DataAnnotations.Schema;

using Fancyx.Core.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Entities.System
{
    [Table("role_dept")]
    [PrimaryKey(nameof(RoleId), nameof(DeptId))]
    public class RoleDept : ITenant
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Column("role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Column("dept_id")]
        public long DeptId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}