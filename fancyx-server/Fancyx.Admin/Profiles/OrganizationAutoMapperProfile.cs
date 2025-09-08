using AutoMapper;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.DataAccess.Entities.Organization;

namespace Fancyx.Admin.Profiles
{
    public class OrganizationAutoMapperProfile : Profile
    {
        public OrganizationAutoMapperProfile()
        {
            CreateMap<DeptDto, Dept>();
            CreateMap<Dept, DeptListDto>();
            CreateMap<PositionGroupDto, PositionGroup>();
            CreateMap<PositionGroup, PositionGroupListDto>()
                .ForMember(d => d.Rawchildren, o => o.MapFrom(s => s.Children));
            CreateMap<Position, PositionListDto>();
            CreateMap<PositionDto, Position>();
        }
    }
}