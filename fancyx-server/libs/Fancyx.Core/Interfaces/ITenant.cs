namespace Fancyx.Core.Interfaces
{
    public interface ITenant
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        string? TenantId { get; set; }
    }
}