using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;
using System.Collections.Generic;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IDictDataService : IScopedDependency
    {
        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddDictDataAsync(AddOrUpdateDictDataRequest req);

        /// <summary>
        /// 字典分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<DictDataItem>> GetDictDataListAsync(GetDictDataListRequest req);

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateDictDataAsync(AddOrUpdateDictDataRequest req);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteDictDataAsync(List<long> ids);

        /// <summary>
        /// 字典详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DictDataItem> GetDictDataAsync(long id);
    }
}