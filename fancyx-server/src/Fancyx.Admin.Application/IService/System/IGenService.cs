using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IGenService : IScopedDependency
    {
        Task<GenCodeResponse> GenCodeAsync(long tableId);

        Task ImportTableAsync(string table);

        Task<PagedResult<TableInfoItem>> GetTableListAsync(GetTableListRequest dto);

        Task GenSyncFromDb(long tableId);

        Task<PagedResult<GenTableItem>> GetGenTableListAsync(GetGenTableListRequest dto);

        Task<PagedResult<GenTableColumnItem>> GetGenTableColumnListAsync(GenTableColumnRequest dto);

        Task DeleteGenTableAsync(long tableId);

        Task SaveGenTableInfoAsync(GenTableInfoDto dto);

        Task SaveGenColumnInfoAsync(List<SaveGenColumnInfoItem> dtos);

        Task<GenDetails> GetGenDetailsInfoAsync(long tableId);
    }
}
