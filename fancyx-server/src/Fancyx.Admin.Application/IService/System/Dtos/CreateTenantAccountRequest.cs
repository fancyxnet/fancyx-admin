using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class CreateTenantAccountRequest
    {
        [Required, NotNull]
        public string? TenantId { get; set; }

        public int ErrCount { get; set; }
    }
}
