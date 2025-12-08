using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo.Models
{
    public class GetCustomerListRequest : PageSearch
    {
        public string? Code { get; set; }
    }
}
