using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Models
{
    public class CreateTenantAccountRequest
    {
        [Required, NotNull]
        public string? TenantId { get; set; }

        public int ErrCount { get; set; }
    }
}
