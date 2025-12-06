using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IDictTypeService : IScopedDependency
    {
        Task AddDictTypeAsync(AddOrUpdateDictTypeRequest dto);

        Task<PagedResult<DictTypeItem>> GetDictTypeListAsync(GetDictTypeListRequest dto);

        Task UpdateDictTypeAsync(AddOrUpdateDictTypeRequest dto);

        Task DeleteDictTypeAsync(string dictType);

        Task<List<AppOption>> GetDictDataOptionsAsync(string type);

        Task DeleteDictTypesAsync(long[] ids);
    }
}