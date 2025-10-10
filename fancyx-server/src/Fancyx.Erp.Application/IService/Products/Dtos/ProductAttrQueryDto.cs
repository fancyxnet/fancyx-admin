using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductAttrQueryDto : PageSearch
    {
        public string? Name { get; set; }
    }
}