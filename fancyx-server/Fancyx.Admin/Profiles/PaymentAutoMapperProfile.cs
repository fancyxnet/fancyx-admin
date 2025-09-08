using AutoMapper;

using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.DataAccess.Entities.Payment;

namespace Fancyx.Admin.Profiles
{
    public class PaymentAutoMapperProfile : Profile
    {
        public PaymentAutoMapperProfile()
        {
            CreateMap<PaymentOrder, PayOrderListDto>();
            CreateMap<PayProvider, PayProviderListDto>();
        }
    }
}