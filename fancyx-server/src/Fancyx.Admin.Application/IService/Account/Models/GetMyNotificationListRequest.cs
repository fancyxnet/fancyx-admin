namespace Fancyx.Admin.Application.IService.Account.Models
{
    public class GetMyNotificationListRequest : PageSearch
    {
        public string? Title { get; set; }

        public bool? IsReaded { get; set; }
    }
}