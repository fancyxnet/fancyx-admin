namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GetConfigListRequest : PageSearch
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
    }
}