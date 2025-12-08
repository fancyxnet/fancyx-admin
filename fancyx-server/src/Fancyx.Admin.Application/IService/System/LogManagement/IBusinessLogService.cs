using Fancyx.Admin.Application.IService.System.LogManagement.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System.LogManagement
{
    public interface IBusinessLogService : IScopedDependency
    {
        /// <summary>
        /// 业务日志分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<BusinessLogItem>> GetBusinessLogListAsync(GetBusinessLogListRequest req);

        /// <summary>
        /// 获取所有业务类型选项
        /// </summary>
        /// <param name="type">业务类型模糊匹配</param>
        /// <returns></returns>
        Task<List<AppOption>> GetBusinessTypeOptionsAsync(string? type);
    }
}