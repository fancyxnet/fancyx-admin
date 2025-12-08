using Fancyx.Admin.Application.IService.Feedback.Models;
using Fancyx.Admin.EfCore.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Feedback
{
    public interface ITicketService : IScopedDependency
    {
        /// <summary>
        /// 创建工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task CreateTicketAsync(TicketCreateRequest req);

        /// <summary>
        /// 关闭工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task CloseTicketAsync(long id);

        /// <summary>
        /// 运营后台工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<TicketItem>> GetTicketListAsync(GetTicketListRequest req);

        /// <summary>
        /// 用户工单列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<UserTicketItem>> GetUserTicketListAsync(GetUserTicketListRequest req);

        /// <summary>
        /// 回复工单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task ReplyTicketAsync(ReplyTicketRequest req);

        /// <summary>
        /// 工单评价
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task EvaluationTicketAsync(EvaluationTicketRequest req);

        /// <summary>
        /// 工单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TicketDetails> GetTicketDetailsAsync(long id);
    }
}