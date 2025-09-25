using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class RolePowerInfoDto
    {
        /// <summary>
        /// 指定的部门ID
        /// </summary>
        public List<Guid>? DeptIds { get; set; }

        /// <summary>
        /// 部门权限类型
        /// </summary>
        public DeptPowerType DeptPowerType { get; set; }
        
        /// <summary>
        /// 所有部门ID
        /// </summary>
        public List<Guid>? AllDeptIds { get; set; }
    }
}