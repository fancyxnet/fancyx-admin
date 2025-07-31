using Fancyx.Admin.IService.Payment;
using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Core.Interfaces;
using Fancyx.Payment;
using Fancyx.Payment.Entities;
using Fancyx.Payment.Enums;
using Fancyx.Payment.Models;
using Fancyx.Repository;

namespace Fancyx.Admin.Service.Payment
{
    public class PayOrderService : IPayOrderService, IScopedDependency
    {
        private readonly IRepository<PaymentOrder> _paymentOrderRepository;
        private readonly IPayNormalize _alipayService;
        private readonly ILogger<PayOrderService> _logger;

        public PayOrderService(IRepository<PaymentOrder> paymentOrderRepository, [FromKeyedServices(PaymentType.AliPay)] IPayNormalize alipayService, ILogger<PayOrderService> logger)
        {
            _paymentOrderRepository = paymentOrderRepository;
            _alipayService = alipayService;
            _logger = logger;
        }

        public async Task PayOrderRefundAsync(RefundRequest req)
        {
            var result = await _alipayService.RefundAsync(req);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("单号{orderNo}退款失败，原因：{@result}", req.OrderNo, result);
                throw new BusinessException($"退款失败: {result.SubMsg}");
            }
        }

        public async Task<PagedResult<PayOrderListDto>> QueryPayOrderAsync(PayOrderQueryDto dto)
        {
            var rows = await _paymentOrderRepository
                .WhereIf(!string.IsNullOrEmpty(dto.OrderNo), x => x.OrderNo == dto.OrderNo)
                .WhereIf(!string.IsNullOrEmpty(dto.RefundNo), x => x.RefundNo == dto.RefundNo)
                .WhereIf(!string.IsNullOrEmpty(dto.PayStatus), x => x.PayStatus == dto.PayStatus)
                .WhereIf(dto.ProviderId.HasValue, x => x.ProviderId == dto.ProviderId)
                .OrderByDescending(x => x.CreationTime)
                .Count(out var total)
                .Page(dto.Current, dto.PageSize)
                .ToListAsync<PayOrderListDto>();

            return new PagedResult<PayOrderListDto>(total, rows);
        }
    }
}