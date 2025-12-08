using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface ITenantService : IScopedDependency
    {
        Task AddTenantAsync(AddOrUpdateTenantRequest req);

        Task<PagedResult<TenantItem>> GetTenantListAsync(GetTenantListRequest req);

        Task UpdateTenantAsync(AddOrUpdateTenantRequest req);

        Task DeleteTenantAsync(string id);

        Task AssignTenantMenuAsync(AssignTenantMenuRequest req);

        Task<List<long>> GetTenantMenuIdsAsync(string id);

        Task<TenantAccountInfo> CreateTenantAccountAsync(CreateTenantAccountRequest req);
    }
}