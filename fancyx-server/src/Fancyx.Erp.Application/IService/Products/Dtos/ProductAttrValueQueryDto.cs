using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductAttrValueQueryDto : PageSearch
    {
        public string? Code { get; set; }
    }
}