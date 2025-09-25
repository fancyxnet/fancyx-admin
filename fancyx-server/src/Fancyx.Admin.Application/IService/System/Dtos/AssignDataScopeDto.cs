using System.ComponentModel.DataAnnotations;
using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignDataScopeDto
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public DeptPowerType DeptPowerType { get; set; }

        public Guid[]? DeptIds { get; set; }
    }
}
