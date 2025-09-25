using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo.Dtos
{
    public class CustomerQueryDto : PageSearch
    {
        public string? Code { get; set; }
    }
}
