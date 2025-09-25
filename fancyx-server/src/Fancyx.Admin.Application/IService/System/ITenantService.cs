using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface ITenantService : IScopedDependency
    {
        Task AddTenantAsync(TenantDto dto);

        Task<PagedResult<TenantResultDto>> GetTenantListAsync(TenantSearchDto dto);

        Task UpdateTenantAsync(TenantDto dto);

        Task DeleteTenantAsync(Guid id);
    }
}