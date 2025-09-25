using AutoMapper;
using Fancyx.Admin.Application.IService.Organization.Dtos;
using Fancyx.Admin.EfCore.Entities.Organization;

namespace Fancyx.Admin.Application.Profiles
{
    public class OrganizationAutoMapperProfile : Profile
    {
        public OrganizationAutoMapperProfile()
        {
            AllowNullCollections = true;

            CreateMap<DeptDto, Dept>();
            CreateMap<Dept, DeptListDto>();
            CreateMap<PositionGroupDto, PositionGroup>();
            CreateMap<PositionGroup, PositionGroupListDto>();
            CreateMap<Position, PositionListDto>();
            CreateMap<PositionDto, Position>();
        }
    }
}