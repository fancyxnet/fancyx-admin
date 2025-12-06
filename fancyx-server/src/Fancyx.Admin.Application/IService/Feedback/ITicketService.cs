using Fancyx.Admin.Application.IService.Feedback.Dtos;
using Fancyx.Admin.EfCore.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Feedback
{
    public interface ITicketService : IScopedDependency
    {
        Task CreateTicketAsync(TicketCreateRequest dto);
        Task CloseTicketAsync(long id);
        Task<PagedResult<TicketItem>> GetTicketListAsync(GetTicketListRequest dto);
        Task<PagedResult<UserTicketItem>> GetUserTicketListAsync(GetUserTicketListRequest dto);
        Task ReplyTicketAsync(ReplyTicketRequest dto);
        Task EvaluationTicketAsync(EvaluationTicketRequest dto);
        Task<TicketDetails> GetTicketDetailsAsync(long id);
    }
}