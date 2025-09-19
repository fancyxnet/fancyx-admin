using AutoMapper;

using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Admin.IService.System.LogManagement.Dtos;
using Fancyx.Logger.Entities;

namespace Fancyx.Admin.Profiles
{
    public class LogAutoMapperProfile : Profile
    {
        public LogAutoMapperProfile()
        {
            CreateMap<ApiAccessLog, ApiAccessLogListDto>();
            CreateMap<ExceptionLog, ExceptionLogListDto>();
            CreateMap<LogRecord, BusinessLogListDto>();
            CreateMap<LoginLog, LoginLogListDto>();
        }
    }
}