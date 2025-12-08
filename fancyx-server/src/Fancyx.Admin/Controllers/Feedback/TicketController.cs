using Fancyx.Admin.Application.IService.Feedback;
using Fancyx.Admin.Application.IService.Feedback.Models;
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
        public async Task<AppResponse<bool>> CreateTicketAsync([FromBody] TicketCreateRequest req)
        {
            await _ticketService.CreateTicketAsync(req);
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
        public async Task<AppResponse<PagedResult<TicketItem>>> GetTicketListAsync([FromQuery] GetTicketListRequest req)
        {
            var data = await _ticketService.GetTicketListAsync(req);
            return Result.Data(data);
        }

        [HttpGet("ListForUser")]
        [HasPermission("Feedback.Ticket.ListForUser")]
        public async Task<AppResponse<PagedResult<UserTicketItem>>> GetUserTicketListAsync([FromQuery] GetUserTicketListRequest req)
        {
            var data = await _ticketService.GetUserTicketListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Reply")]
        [HasPermission("Feedback.Ticket.Reply")]
        public async Task<AppResponse<bool>> ReplyTicketAsync([FromBody] ReplyTicketRequest req)
        {
            await _ticketService.ReplyTicketAsync(req);
            return Result.Ok();
        }

        [HttpPost("Evaluation")]
        [HasPermission("Feedback.Ticket.Evaluation")]
        public async Task<AppResponse<bool>> EvaluationTicketAsync([FromBody] EvaluationTicketRequest req)
        {
            await _ticketService.EvaluationTicketAsync(req);
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