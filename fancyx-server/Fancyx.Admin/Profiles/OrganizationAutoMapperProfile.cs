using AutoMapper;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.DataAccess.Entities.Organization;

namespace Fancyx.Admin.Profiles
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