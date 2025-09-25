using System.ComponentModel.DataAnnotations;
using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignDataScopeDto
    {
        [Required]
        public long RoleId { get; set; }

        [Required]
        public DeptPowerType DeptPowerType { get; set; }

        public long[]? DeptIds { get; set; }
    }
}
