using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Models
{
    public class GetProductCategoryListRequest : PageSearch
    {
        public string? Name { get; set; }
    }
}