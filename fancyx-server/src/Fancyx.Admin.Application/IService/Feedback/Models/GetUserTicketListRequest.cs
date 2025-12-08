namespace Fancyx.Admin.Application.IService.Feedback.Models
{
    public class GetUserTicketListRequest : PageSearch
    {
        public string? Title { get; set; }
    }
}