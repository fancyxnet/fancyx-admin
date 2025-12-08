using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.Products.Models
{
    public class GetProductAttrValueListRequest : PageSearch
    {
        public string? Code { get; set; }
    }
}