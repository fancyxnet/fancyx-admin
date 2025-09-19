using AutoMapper;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.IService.Account.Dtos;
using Fancyx.Admin.IService.System.Dtos;

namespace Fancyx.Admin.Profiles
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