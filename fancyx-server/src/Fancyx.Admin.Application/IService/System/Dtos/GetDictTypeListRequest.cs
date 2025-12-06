namespace Fancyx.Admin.Application.IService.System.Dtos;

public class GetDictTypeListRequest : PageSearch
{
    public string? Name { get; set; }

    public string? DictType { get; set; }
}