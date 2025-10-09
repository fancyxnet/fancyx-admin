using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductQueryDto : PageSearch
    {
        public string? Name { get; set; }
    }
}