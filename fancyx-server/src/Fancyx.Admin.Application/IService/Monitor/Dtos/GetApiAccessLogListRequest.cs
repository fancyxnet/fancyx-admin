namespace Fancyx.Admin.Application.IService.Monitor.Dtos
{
    public class GetApiAccessLogListRequest : PageSearch
    {
        public string? UserName { get; set; }
        public string? Path { get; set; }
    }
}