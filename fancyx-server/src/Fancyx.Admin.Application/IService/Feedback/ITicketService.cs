using Fancyx.Admin.Application.IService.Feedback.Models;
using Fancyx.Admin.EfCore.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Feedback
{
    public interface ITicketService : IScopedDependency
    {
        Task CreateTicketAsync(TicketCreateRequest req);
        Task CloseTicketAsync(long id);
        Task<PagedResult<TicketItem>> GetTicketListAsync(GetTicketListRequest req);
        Task<PagedResult<UserTicketItem>> GetUserTicketListAsync(GetUserTicketListRequest req);
        Task ReplyTicketAsync(ReplyTicketRequest req);
        Task EvaluationTicketAsync(EvaluationTicketRequest req);
        Task<TicketDetails> GetTicketDetailsAsync(long id);
    }
}