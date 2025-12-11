using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.System.Models
{
    public class RoleDetails : RoleItem
    {
        /// <summary>
        /// 拥有菜单ID
        /// </summary>
        public long[]? MenuIds { get; set; }

        /// <summary>
        /// 指定的部门ID
        /// </summary>
        public List<long>? DeptIds { get; set; }

        /// <summary>
        /// 部门权限类型
        /// </summary>
        public DeptPowerType DeptPowerType { get; set; }
    }
}
