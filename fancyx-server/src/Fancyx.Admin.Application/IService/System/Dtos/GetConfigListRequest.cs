namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GetConfigListRequest : PageSearch
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
    }
}