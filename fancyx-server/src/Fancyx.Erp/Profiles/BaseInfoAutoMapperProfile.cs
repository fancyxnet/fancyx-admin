using AutoMapper;

using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.IService.BaseInfo.Dtos;

namespace Fancyx.Erp.Profiles
{
    public class BaseInfoAutoMapperProfile : Profile
    {
        public BaseInfoAutoMapperProfile()
        {
            CreateMap<CustomerDto, Customer>(MemberList.None);
        }
    }
}
