using Fancyx.Shared.Models;

namespace Fancyx.Erp.IService.BaseInfo.Dtos
{
    public class CustomerQueryDto : PageSearch
    {
        public string? Code { get; set; }
    }
}
