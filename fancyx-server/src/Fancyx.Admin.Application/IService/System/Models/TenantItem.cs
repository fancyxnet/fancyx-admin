namespace Fancyx.Admin.Application.IService.System.Models;

public class TenantItem
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 租户标识
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 租户域名
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; set; }

    public bool IsEnabled { get; set; }
}