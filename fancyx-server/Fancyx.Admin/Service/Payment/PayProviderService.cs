using AutoMapper;
using Fancyx.Admin.IService.Payment;
using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Core.Extensions;
using Fancyx.Core.Utils;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Payment;

namespace Fancyx.Admin.Service.Payment
{
    public class PayProviderService : IPayProviderService
    {
        private readonly IRepository<PayProvider> _payProviderRepository;
        private readonly IRepository<PaymentOrder> _paymentOrderRepository;
        private readonly IMapper _autoMapper;

        public PayProviderService(IRepository<PayProvider> payProviderRepository, IRepository<PaymentOrder> paymentOrderRepository, IMapper autoMapper)
        {
            _payProviderRepository = payProviderRepository;
            _paymentOrderRepository = paymentOrderRepository;
            _autoMapper = autoMapper;
        }

        public async Task AddPayProviderAsync(PayProviderDto dto)
        {
            if (await _payProviderRepository.AnyAsync(x => x.Name == dto.Name))
            {
                throw new BusinessException($"支付渠道{dto.Name}已存在");
            }

            var entity = _autoMapper.Map<PayProviderDto, PayProvider>(dto);
            await _payProviderRepository.InsertAsync(entity);
        }

        public async Task DeletePayProviderAsync(Guid id)
        {
            if (await _paymentOrderRepository.AnyAsync(x => x.ProviderId == id))
            {
                throw new BusinessException("该支付渠道已存在支付订单，无法删除");
            }

            await _payProviderRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<PayProviderListDto>> GetPayProviderListAsync(PayProviderQueryDto dto)
        {
            var resp = await _payProviderRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.Contains(dto.Name!))
                .WhereIf(dto.Type.HasValue, x => (int)x.Type == dto.Type)
                .WhereIf(dto.IsEnabled.HasValue, x => x.IsEnabled == dto.IsEnabled)
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<PayProviderListDto>(resp.Total, resp.Items.MapperList<PayProvider, PayProviderListDto>());
        }

        public async Task UpdatePayProviderAsync(PayProviderDto dto)
        {
            var entity = await _payProviderRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            if (entity.Name != dto.Name && await _payProviderRepository.AnyAsync(x => x.Name == dto.Name))
            {
                throw new BusinessException($"支付渠道{dto.Name}已存在");
            }
            ReflectionUtils.CopyTo(dto, entity, nameof(entity.Id));
            await _payProviderRepository.UpdateAsync(entity);
        }
    }
}