using Fancyx.Admin.IService.Payment;
using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Core.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Payment
{
    [Route("api/[controller]")]
    public class PayProviderController : ControllerBase
    {
        private readonly IPayProviderService _payProviderService;

        public PayProviderController(IPayProviderService payProviderService)
        {
            _payProviderService = payProviderService;
        }

        /// <summary>
        /// 新增支付渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Payment.PayProvider.Add")]
        public async Task<AppResponse<bool>> AddPayProviderAsync([FromBody] PayProviderDto dto)
        {
            await _payProviderService.AddPayProviderAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 支付渠道分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Payment.PayProvider.List")]
        public async Task<AppResponse<PagedResult<PayProviderListDto>>> GetPayProviderListAsync([FromQuery] PayProviderQueryDto dto)
        {
            var data = await _payProviderService.GetPayProviderListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改支付渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Payment.PayProvider.Update")]
        public async Task<AppResponse<bool>> UpdatePayProviderAsync([FromBody] PayProviderDto dto)
        {
            await _payProviderService.UpdatePayProviderAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除支付渠道
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [HasPermission("Payment.PayProvider.Add")]
        public async Task<AppResponse<bool>> DeletePayProviderAsync(Guid id)
        {
            await _payProviderService.DeletePayProviderAsync(id);
            return Result.Ok();
        }
    }
}