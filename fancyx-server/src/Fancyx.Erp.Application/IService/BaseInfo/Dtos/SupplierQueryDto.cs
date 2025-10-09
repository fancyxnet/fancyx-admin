using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo.Dtos
{
    public class SupplierQueryDto : PageSearch
    {
        public string? Name { get; set; }
    }
}