using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.Feedback.Dtos
{
    public class TicketCreateRequest
    {
        [NotNull]
        [Required]
        public string? Title { get; set; }

        [NotNull]
        [Required]
        public string? Content { get; set; }
    }
}