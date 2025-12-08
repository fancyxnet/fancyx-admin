namespace Fancyx.Admin.Application.IService.Monitor.Models
{
    public class GetExceptionLogListRequest : PageSearch
    {
        public string? UserName { get; set; }

        public string? Path { get; set; }

        public bool? IsHandled { get; set; }
    }
}