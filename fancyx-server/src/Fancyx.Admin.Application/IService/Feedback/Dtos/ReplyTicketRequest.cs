using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.Feedback.Dtos
{
    public class ReplyTicketRequest
    {
        public long TicketId { get; set; }

        [Required, NotNull]
        public string? Content { get; set; }
    }
}