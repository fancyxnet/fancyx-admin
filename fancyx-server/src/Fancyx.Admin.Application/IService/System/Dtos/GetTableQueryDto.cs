namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GetTableQueryDto : PageSearch
    {
        public string? TableName { get; set; }
    }
}
