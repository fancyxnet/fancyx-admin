namespace Fancyx.Admin.Application.IService.System.Models;

public class GetDictTypeListRequest : PageSearch
{
    public string? Name { get; set; }

    public string? DictType { get; set; }
}