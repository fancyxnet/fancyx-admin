using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.System
{
    public interface IConfigService : IScopedDependency
    {
        Task AddConfigAsync(ConfigDto dto);

        Task<PagedResult<ConfigListDto>> GetConfigListAsync(ConfigQueryDto dto);

        Task UpdateConfigAsync(ConfigDto dto);

        Task DeleteConfigAsync(Guid id);
    }
}