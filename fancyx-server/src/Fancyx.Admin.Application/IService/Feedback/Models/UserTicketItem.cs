using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.Feedback.Models
{
    public class UserTicketItem
    {
        public long Id { get; set; }
        public int ReplyCount { get; set; }
        public string Title { get; set; } = null!;
        public TicketStatus Status { get; set; }
        public int Rating { get; set; }
        public string? RatingComment { get; set; }
        public DateTime CreationTime { get; set; }
    }
}