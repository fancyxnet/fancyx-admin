using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IConfigService : IScopedDependency
    {
        Task AddConfigAsync(AddOrUpdateConfigRequest dto);

        Task<PagedResult<ConfigItem>> GetConfigListAsync(GetConfigListRequest dto);

        Task UpdateConfigAsync(AddOrUpdateConfigRequest dto);

        Task DeleteConfigAsync(long id);
    }
}