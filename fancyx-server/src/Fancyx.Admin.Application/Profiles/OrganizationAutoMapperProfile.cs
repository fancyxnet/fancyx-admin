using AutoMapper;
using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Admin.EfCore.Entities.Organization;

namespace Fancyx.Admin.Application.Profiles
{
    public class OrganizationAutoMapperProfile : Profile
    {
        public OrganizationAutoMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<AddOrUpdateDeptRequest, Dept>();
            CreateMap<Dept, DeptItem>();
            CreateMap<AddOrUpdatePositionGroupRequest, PositionGroup>();
            CreateMap<PositionGroup, PositionGroupItem>();
            CreateMap<Position, PositionItem>();
            CreateMap<AddOrUpdatePositionRequest, Position>();
        }
    }
}