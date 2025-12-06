namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GetTableListRequest : PageSearch
    {
        public string? TableName { get; set; }
    }
}
