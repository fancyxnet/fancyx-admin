using AutoMapper;
using Fancyx.Admin.Application.IService.Account.Models;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.EfCore.Entities.Gen;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Models;

namespace Fancyx.Admin.Application.Profiles
{
    public class SystemAutoMapperProfile : Profile
    {
        public SystemAutoMapperProfile()
        {
            CreateMap<TokenResponse, LoginRespone>();
            CreateMap<AddOrUpdateMenuRequest, Menu>();
            CreateMap<Menu, MenuItem>();
            CreateMap<AddOrUpdateDictDataRequest, DictData>();
            CreateMap<Menu, FrontendMenu>();
            CreateMap<Notification, UserNotificationItem>();
            CreateMap<Config, ConfigItem>();
            CreateMap<DictData, DictDataItem>();
            CreateMap<User, UserDetails>(MemberList.None);
            CreateMap<TableInfo, TableInfoItem>(MemberList.None);
            CreateMap<GenTable, GenTableItem>(MemberList.None);
            CreateMap<GenTableColumn, GenTableColumnItem>(MemberList.None);
            CreateMap<SaveGenTableInfoRequest, GenTable>(MemberList.None);
            CreateMap<GenTableColumnRequest, GenTableColumn>(MemberList.None);
            CreateMap<GenTable, GenDetails>(MemberList.None);
        }
    }
}