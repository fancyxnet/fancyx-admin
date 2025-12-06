using Fancyx.Admin.Application.IService.Feedback;
using Fancyx.Admin.Application.IService.Feedback.Dtos;
using Fancyx.Admin.EfCore.Models;
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
        public async Task<AppResponse<bool>> CreateTicketAsync([FromBody] TicketCreateRequest dto)
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
        public async Task<AppResponse<PagedResult<TicketItem>>> GetTicketListAsync([FromQuery] GetTicketListRequest dto)
        {
            var data = await _ticketService.GetTicketListAsync(dto);
            return Result.Data(data);
        }

        [HttpGet("ListForUser")]
        [HasPermission("Feedback.Ticket.ListForUser")]
        public async Task<AppResponse<PagedResult<UserTicketItem>>> GetUserTicketListAsync([FromQuery] GetUserTicketListRequest dto)
        {
            var data = await _ticketService.GetUserTicketListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Reply")]
        [HasPermission("Feedback.Ticket.Reply")]
        public async Task<AppResponse<bool>> ReplyTicketAsync([FromBody] ReplyTicketRequest dto)
        {
            await _ticketService.ReplyTicketAsync(dto);
            return Result.Ok();
        }

        [HttpPost("Evaluation")]
        [HasPermission("Feedback.Ticket.Evaluation")]
        public async Task<AppResponse<bool>> EvaluationTicketAsync([FromBody] EvaluationTicketRequest dto)
        {
            await _ticketService.EvaluationTicketAsync(dto);
            return Result.Ok();
        }

        [HttpGet("Details")]
        [HasPermission("Feedback.Ticket.Details")]
        public async Task<AppResponse<TicketDetails>> GetTicketDetailsAsync(long id)
        {
            var data = await _ticketService.GetTicketDetailsAsync(id);
            return Result.Data(data);
        }
    }
}