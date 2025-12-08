using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo.Models
{
    public class GetSupplierListRequest : PageSearch
    {
        public string? Name { get; set; }
    }
}