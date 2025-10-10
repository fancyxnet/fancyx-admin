using AutoMapper;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;

namespace Fancyx.Erp.Application.Profiles
{
    public class BaseInfoAutoMapperProfile : Profile
    {
        public BaseInfoAutoMapperProfile()
        {
            CreateMap<CustomerDto, Customer>(MemberList.None);
            CreateMap<Warehouse, StoreHouseListDto>(MemberList.None);
        }
    }
}