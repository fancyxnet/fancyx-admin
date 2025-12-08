using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo.Models
{
    public class GetWarehouseListRequest : PageSearch
    {
        public string? Name { get; set; }
    }
}