﻿using AutoMapper;

using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.IService.Organization.Dtos;

namespace Fancyx.Admin.Profiles
{
    public class OrganizationAutoMapperProfile : Profile
    {
        public OrganizationAutoMapperProfile()
        {
            CreateMap<EmployeeDto, EmployeeDO>();
            CreateMap<DeptDto, DeptDO>();
            CreateMap<DeptDO, DeptListDto>();
            CreateMap<PositionGroupDto, PositionGroupDO>();
            CreateMap<PositionGroupDO, PositionGroupListDto>()
                .ForMember(d => d.Rawchildren, o => o.MapFrom(s => s.Children));
            CreateMap<PositionDO, PositionListDto>();
            CreateMap<PositionDto, PositionDO>();
            CreateMap<EmployeeDO, EmployeeInfoDto>();
        }
    }
}