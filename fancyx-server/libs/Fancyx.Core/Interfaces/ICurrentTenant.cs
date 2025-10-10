namespace Fancyx.Core.Interfaces
{
    public interface ICurrentTenant
    {
        string? TenantId { get; }
    }
}