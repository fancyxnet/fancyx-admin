using Fancyx.Admin.Application.IService.System.Models;
using Cracker.AspNetCore.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IDictTypeService : IScopedDependency
    {
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AddDictTypeAsync(AddOrUpdateDictTypeRequest req);

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<DictTypeItem>> GetDictTypeListAsync(GetDictTypeListRequest req);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdateDictTypeAsync(AddOrUpdateDictTypeRequest req);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="dictType"></param>
        /// <returns></returns>
        Task DeleteDictTypeAsync(string dictType);

        /// <summary>
        /// 字典选项
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<AppOption>> GetDictDataOptionsAsync(string type);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteDictTypesAsync(List<long> ids);

        /// <summary>
        /// 字典详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DictTypeItem> GetDictTypeAsync(long id);
    }
}