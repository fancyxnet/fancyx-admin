using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignTenantMenuDto
    {
        [Required]
        public string? TenantId { get; set; }

        public long[]? MenuIds { get; set; }
    }
}
