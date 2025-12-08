namespace Fancyx.Admin.Application.IService.Feedback.Models
{
    public class TicketDetails : UserTicketItem
    {
        public string? Content { get; set; }
        public List<TicketReplyListDto>? ReplyList { get; set; }
    }

    public class TicketReplyListDto
    {
        public DateTime CreationTime { get; set; }
        public string? Content { get; set; }
        public bool IsAssignedUser { get; set; }
        public string? AssignedNickName { get; set; }
    }
}