using AutoMapper;
using Fancyx.Admin.Application.IService.Monitor.Models;
using Fancyx.Admin.Application.IService.System.LogManagement.Models;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Shared.Logger.Entities;

namespace Fancyx.Admin.Application.Profiles
{
    public class LogAutoMapperProfile : Profile
    {
        public LogAutoMapperProfile()
        {
            CreateMap<ApiAccessLog, ApiAccessLogItem>();
            CreateMap<ExceptionLog, ExceptionLogItem>();
            CreateMap<LogRecord, BusinessLogItem>();
            CreateMap<LoginLog, LoginLogItem>();
        }
    }
}