using Fancyx.Admin.Application.IService.Feedback;
using Fancyx.Admin.Application.IService.Feedback.Dtos;
using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Feedback
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerGroup("客户反馈")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("Create")]
        [HasPermission("Feedback.Ticket.UserCreate")]
        public async Task<AppResponse<bool>> CreateTicketAsync([FromBody] TicketCreateDto dto)
        {
            await _ticketService.CreateTicketAsync(dto);
            return Result.Ok();
        }

        [HttpPost("Close")]
        [HasPermission("Feedback.Ticket.Close")]
        public async Task<AppResponse<bool>> CloseTicketAsync(long id)
        {
            await _ticketService.CloseTicketAsync(id);
            return Result.Ok();
        }

        [HttpGet("ListForAdmin")]
        [HasPermission("Feedback.Ticket.ListForAdmin")]
        public async Task<AppResponse<PagedResult<TicketListDto>>> GetTicketListAsync([FromQuery] TicketQueryDto dto)
        {
            var data = await _ticketService.GetTicketListAsync(dto);
            return Result.Data(data);
        }

        [HttpGet("ListForUser")]
        [HasPermission("Feedback.Ticket.ListForUser")]
        public async Task<AppResponse<PagedResult<UserTicketListDto>>> GetUserTicketListAsync([FromQuery] UserTicketQueryDto dto)
        {
            var data = await _ticketService.GetUserTicketListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Reply")]
        [HasPermission("Feedback.Ticket.Reply")]
        public async Task<AppResponse<bool>> ReplyTicketAsync([FromBody] ReplyTicketDto dto)
        {
            await _ticketService.ReplyTicketAsync(dto);
            return Result.Ok();
        }

        [HttpPost("Evaluation")]
        [HasPermission("Feedback.Ticket.Evaluation")]
        public async Task<AppResponse<bool>> EvaluationTicketAsync([FromBody] EvaluationTicketDto dto)
        {
            await _ticketService.EvaluationTicketAsync(dto);
            return Result.Ok();
        }
    }
}