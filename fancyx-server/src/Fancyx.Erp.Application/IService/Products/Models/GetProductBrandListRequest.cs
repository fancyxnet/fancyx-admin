using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Models
{
    public class GetProductBrandListRequest : PageSearch
    {
        public string? Name { get; set; }
    }
}