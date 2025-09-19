namespace Fancyx.Admin.IService.System.Dtos
{
    public class ConfigQueryDto : PageSearch
    {
        public string? Name { get; set; }
        public string? Key { get; set; }
    }
}