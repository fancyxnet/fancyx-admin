using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductCategoryQueryDto : PageSearch
    {
        public string? Name { get; set; }
    }
}