using AutoMapper;
using Fancyx.Admin.Application.IService.Monitor.Dtos;
using Fancyx.Admin.Application.IService.System.LogManagement.Dtos;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Logger.Entities;

namespace Fancyx.Admin.Application.Profiles
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