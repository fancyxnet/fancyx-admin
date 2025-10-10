namespace Fancyx.Admin.Application.IService.System.Dtos;

public class DictTypeSearchDto : PageSearch
{
    public string? Name { get; set; }

    public string? DictType { get; set; }
}