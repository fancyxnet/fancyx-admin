namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class NotificationQueryDto : PageSearch
    {
        public string? Title { get; set; }

        public bool? IsReaded { get; set; }
    }
}