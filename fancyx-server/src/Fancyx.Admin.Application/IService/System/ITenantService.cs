using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface ITenantService : IScopedDependency
    {
        Task AddTenantAsync(AddOrUpdateTenantRequest dto);

        Task<PagedResult<TenantItem>> GetTenantListAsync(GetTenantListRequest dto);

        Task UpdateTenantAsync(AddOrUpdateTenantRequest dto);

        Task DeleteTenantAsync(string id);

        Task AssignTenantMenuAsync(AssignTenantMenuRequest dto);

        Task<List<long>> GetTenantMenuIdsAsync(string id);

        Task<TenantAccountInfo> CreateTenantAccountAsync(CreateTenantAccountRequest dto);
    }
}