namespace Fancyx.Admin.EfCore.Models
{
    public class TicketReplyInfo
    {
        public DateTime CreationTime { get; set; }
        public string? Content { get; set; }
        public bool IsAssignedUser { get; set; }
        public string? AssignedNickName { get; set; }
    }
}