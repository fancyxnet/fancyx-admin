using Fancyx.Admin.IService.System.LogManagement.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.System.LogManagement
{
    public interface ILoginLogService : IScopedDependency
    {
        /// <summary>
        /// 登录日志分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<LoginLogListDto>> GetLoginLogListAsync(LoginLogQueryDto dto);
    }
}