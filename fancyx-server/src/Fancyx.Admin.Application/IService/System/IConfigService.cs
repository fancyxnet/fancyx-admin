using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IConfigService : IScopedDependency
    {
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AddConfigAsync(AddOrUpdateConfigRequest req);

        /// <summary>
        /// 配置列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<ConfigItem>> GetConfigListAsync(GetConfigListRequest req);

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdateConfigAsync(AddOrUpdateConfigRequest req);

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteConfigAsync(long id);
    }
}