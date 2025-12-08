using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IGenService : IScopedDependency
    {
        Task<GenCodeResponse> GenCodeAsync(long tableId);

        Task ImportTableAsync(string table);

        Task<PagedResult<TableInfoItem>> GetTableListAsync(GetTableListRequest req);

        Task GenSyncFromDb(long tableId);

        Task<PagedResult<GenTableItem>> GetGenTableListAsync(GetGenTableListRequest req);

        Task<PagedResult<GenTableColumnItem>> GetGenTableColumnListAsync(GenTableColumnRequest req);

        Task DeleteGenTableAsync(long tableId);

        Task SaveGenTableInfoAsync(SaveGenTableInfoRequest req);

        Task SaveGenColumnInfoAsync(List<SaveGenColumnInfoItem> dtos);

        Task<GenDetails> GetGenDetailsInfoAsync(long tableId);
    }
}
