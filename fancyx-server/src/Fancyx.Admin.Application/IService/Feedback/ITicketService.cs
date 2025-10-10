using Fancyx.Admin.Application.IService.Feedback.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.Feedback
{
    public interface ITicketService : IScopedDependency
    {
        Task CreateTicketAsync(TicketCreateDto dto);
        Task CloseTicketAsync(long id);
        Task<PagedResult<TicketListDto>> GetTicketListAsync(TicketQueryDto dto);
        Task<PagedResult<UserTicketListDto>> GetUserTicketListAsync(UserTicketQueryDto dto);
        Task ReplyTicketAsync(ReplyTicketDto dto);
        Task EvaluationTicketAsync(EvaluationTicketDto dto);
        Task<TicketDetailsDto> GetTicketDetailsAsync(long id);
    }
}