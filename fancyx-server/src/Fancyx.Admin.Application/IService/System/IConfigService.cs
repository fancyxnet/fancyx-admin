using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IConfigService : IScopedDependency
    {
        Task AddConfigAsync(ConfigDto dto);

        Task<PagedResult<ConfigListDto>> GetConfigListAsync(ConfigQueryDto dto);

        Task UpdateConfigAsync(ConfigDto dto);

        Task DeleteConfigAsync(long id);
    }
}