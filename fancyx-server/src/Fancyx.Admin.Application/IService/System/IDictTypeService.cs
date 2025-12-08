using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IDictTypeService : IScopedDependency
    {
        Task AddDictTypeAsync(AddOrUpdateDictTypeRequest req);

        Task<PagedResult<DictTypeItem>> GetDictTypeListAsync(GetDictTypeListRequest req);

        Task UpdateDictTypeAsync(AddOrUpdateDictTypeRequest req);

        Task DeleteDictTypeAsync(string dictType);

        Task<List<AppOption>> GetDictDataOptionsAsync(string type);

        Task DeleteDictTypesAsync(long[] ids);
    }
}