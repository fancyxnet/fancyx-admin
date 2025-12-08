using Fancyx.Admin.Application.IService.System.LogManagement.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System.LogManagement
{
    public interface ILoginLogService : IScopedDependency
    {
        /// <summary>
        /// 登录日志分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<LoginLogItem>> GetLoginLogListAsync(GetLoginLogListRequest req);
    }
}