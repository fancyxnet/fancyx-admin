using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignTenantMenuRequest
    {
        [Required, NotNull]
        public string? TenantId { get; set; }

        public long[]? MenuIds { get; set; }
    }
}
