namespace Fancyx.Admin.IService.Monitor.Dtos
{
    public class ExceptionLogQueryDto : PageSearch
    {
        public string? UserName { get; set; }

        public string? Path { get; set; }

        public bool? IsHandled { get; set; }
    }
}