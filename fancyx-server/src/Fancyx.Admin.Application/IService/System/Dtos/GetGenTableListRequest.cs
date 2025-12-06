namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GetGenTableListRequest : PageSearch
    {
        public string? TableName { get; set; }
    }
}
