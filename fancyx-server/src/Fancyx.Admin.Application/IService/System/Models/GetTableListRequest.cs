namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GetTableListRequest : PageSearch
    {
        public string? TableName { get; set; }
    }
}
