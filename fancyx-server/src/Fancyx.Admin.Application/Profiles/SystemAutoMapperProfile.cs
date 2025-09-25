using AutoMapper;
using Fancyx.Admin.Application.IService.Account.Dtos;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore.Entities.System;

namespace Fancyx.Admin.Application.Profiles
{
    public class SystemAutoMapperProfile : Profile
    {
        public SystemAutoMapperProfile()
        {
            CreateMap<TokenResultDto, LoginResultDto>();
            CreateMap<MenuDto, Menu>();
            CreateMap<Menu, MenuListDto>();
            CreateMap<DictDataDto, DictData>();
            CreateMap<Menu, FrontendMenu>();
            CreateMap<Notification, UserNotificationListDto>();
            CreateMap<Config, ConfigListDto>();
            CreateMap<DictData, DictDataListDto>();
            CreateMap<User, UserEditInfoDto>(MemberList.None);
        }
    }
}