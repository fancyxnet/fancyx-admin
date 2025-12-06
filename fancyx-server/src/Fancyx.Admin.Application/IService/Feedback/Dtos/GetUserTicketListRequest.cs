namespace Fancyx.Admin.Application.IService.Feedback.Dtos
{
    public class GetUserTicketListRequest : PageSearch
    {
        public string? Title { get; set; }
    }
}