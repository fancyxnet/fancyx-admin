using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IGenService : IScopedDependency
    {
        Task<GenCodeResultDto> GenCodeAsync(long tableId);

        Task ImportTableAsync(string table);

        Task<PagedResult<TableInfoDto>> GetTableListAsync(GetTableQueryDto dto);

        Task GenSyncFromDb(long tableId);

        Task<PagedResult<GenTableListDto>> GetGenTableListAsync(GenTableQueryDto dto);

        Task<PagedResult<GenTableColumnListDto>> GetGenTableColumnListAsync(GenTableColumnQueryDto dto);

        Task DeleteGenTableAsync(long tableId);

        Task SaveGenTableInfoAsync(GenTableInfoDto dto);

        Task SaveGenColumnInfoAsync(List<GenTableColumnDto> dtos);

        Task<GenDetailsInfoDto> GetGenDetailsInfoAsync(long tableId);
    }
}
