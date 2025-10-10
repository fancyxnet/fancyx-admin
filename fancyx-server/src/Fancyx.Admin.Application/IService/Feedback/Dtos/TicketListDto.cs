using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.Application.IService.Feedback.Dtos
{
    public class TicketListDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public TicketStatus Status { get; set; }
        public int Rating { get; set; }
        public string? RatingComment { get; set; }
        public string? SenderNickName { get; set; }
        public string? AssignedNickName { get; set; }
        public bool IsReply { get; set; }
        public DateTime CreationTime { get; set; }
    }
}