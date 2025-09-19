namespace Fancyx.Admin.IService.System.Dtos;

public class TenantSearchDto : PageSearch
{
    /// <summary>
    /// 租户名称/标识
    /// </summary>
    public string? Keyword { get; set; }
}