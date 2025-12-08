using AutoMapper;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entites;

namespace Fancyx.Erp.Application.Profiles
{
    public class BaseInfoAutoMapperProfile : Profile
    {
        public BaseInfoAutoMapperProfile()
        {
            CreateMap<AddOrUpdateCustomerRequest, Customer>(MemberList.None);
            CreateMap<Warehouse, WarehouseItem>(MemberList.None);
        }
    }
}