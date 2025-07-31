using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Payment.Models;

namespace Fancyx.Admin.IService.Payment
{
    public interface IPayOrderService
    {
        Task<PagedResult<PayOrderListDto>> QueryPayOrderAsync(PayOrderQueryDto dto);

        Task PayOrderRefundAsync(RefundRequest req);
    }
}