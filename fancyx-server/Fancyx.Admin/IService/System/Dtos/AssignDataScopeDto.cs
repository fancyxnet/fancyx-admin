using System.ComponentModel.DataAnnotations;
using Fancyx.Repository.Enums;

namespace Fancyx.Admin.IService.System.Dtos
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
