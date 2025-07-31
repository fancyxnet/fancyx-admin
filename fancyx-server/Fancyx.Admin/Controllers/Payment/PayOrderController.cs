using Fancyx.Admin.IService.Payment;
using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Payment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOrderController : ControllerBase
    {
        private readonly IPayOrderService _payOrderService;

        public PayOrderController(IPayOrderService payOrderService)
        {
            _payOrderService = payOrderService;
        }

        [HttpGet("List")]
        [HasPermission("Payment.Order.List")]
        public async Task<AppResponse<PagedResult<PayOrderListDto>>> QueryPayOrderAsync([FromQuery] PayOrderQueryDto dto)
        {
            var data = await _payOrderService.QueryPayOrderAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Refund")]
        [HasPermission("Payment.Order.Refund")]
        public async Task<AppResponse<bool>> PayOrderRefundAsync([FromBody] RefundRequest req)
        {
            await _payOrderService.PayOrderRefundAsync(req);
            return Result.Ok();
        }
    }
}