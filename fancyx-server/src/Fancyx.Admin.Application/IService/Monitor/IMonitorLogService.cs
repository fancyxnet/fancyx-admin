using Fancyx.Admin.Application.IService.Monitor.Models;

namespace Fancyx.Admin.Application.IService.Monitor
{
    public interface IMonitorLogService
    {
        /// <summary>
        /// API访问日志列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<ApiAccessLogItem>> GetApiAccessLogListAsync(GetApiAccessLogListRequest req);

        /// <summary>
        /// 异常日志列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<ExceptionLogItem>> GetExceptionLogListAsync(GetExceptionLogListRequest req);

        /// <summary>
        /// 标记异常已处理
        /// </summary>
        /// <param name="exceptionId">异常日志ID</param>
        /// <returns></returns>
        Task HandleExceptionAsync(long exceptionId);
    }
}
