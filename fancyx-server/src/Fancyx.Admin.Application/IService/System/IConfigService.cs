using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IConfigService : IScopedDependency
    {
        Task AddConfigAsync(AddOrUpdateConfigRequest req);

        Task<PagedResult<ConfigItem>> GetConfigListAsync(GetConfigListRequest req);

        Task UpdateConfigAsync(AddOrUpdateConfigRequest req);

        Task DeleteConfigAsync(long id);
    }
}