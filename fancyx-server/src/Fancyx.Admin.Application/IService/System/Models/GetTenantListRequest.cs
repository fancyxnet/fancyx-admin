namespace Fancyx.Admin.Application.IService.System.Models;

public class GetTenantListRequest : PageSearch
{
    /// <summary>
    /// 租户名称/标识
    /// </summary>
    public string? Keyword { get; set; }
}