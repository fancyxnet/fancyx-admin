using AutoMapper;
using Fancyx.Admin.Application.IService.Account.Dtos;
using Fancyx.Admin.Application.IService.System.Dtos;
using Fancyx.Admin.EfCore.Entities.Gen;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Models;

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
            CreateMap<TableInfo, TableInfoDto>(MemberList.None);
            CreateMap<GenTable, GenTableListDto>(MemberList.None);
            CreateMap<GenTableColumn, GenTableColumnListDto>(MemberList.None);
            CreateMap<GenTableInfoDto, GenTable>(MemberList.None);
            CreateMap<GenTableColumnDto, GenTableColumn>(MemberList.None);
            CreateMap<GenTable, GenDetailsInfoDto>(MemberList.None);
        }
    }
}