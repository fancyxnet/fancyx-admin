using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IGenService : IScopedDependency
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        Task<GenCodeResponse> GenCodeAsync(long tableId);

        /// <summary>
        /// 导入数据表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Task ImportTableAsync(string table);

        /// <summary>
        /// 表信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<TableInfoItem>> GetTableListAsync(GetTableListRequest req);

        /// <summary>
        /// 同步生成表（从数据库表）
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        Task GenSyncFromDb(long tableId);

        /// <summary>
        /// 生成表列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<GenTableItem>> GetGenTableListAsync(GetGenTableListRequest req);

        /// <summary>
        /// 生成表列信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<GenTableColumnItem>> GetGenTableColumnListAsync(GenTableColumnRequest req);

        /// <summary>
        /// 删除生成表
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        Task DeleteGenTableAsync(long tableId);

        /// <summary>
        /// 保存生成表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task SaveGenTableInfoAsync(SaveGenTableInfoRequest req);

        /// <summary>
        /// 保存生成列信息
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        Task SaveGenColumnInfoAsync(List<SaveGenColumnInfoItem> dtos);

        /// <summary>
        /// 生成表详情
        /// </summary>
        /// <param name="tableId"></param>
        /// <returns></returns>
        Task<GenDetails> GetGenDetailsInfoAsync(long tableId);
    }
}
