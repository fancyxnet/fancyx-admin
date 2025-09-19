using AutoMapper;
using Fancyx.Admin.EfCore.Entities.Log;
using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Admin.IService.System.LogManagement.Dtos;

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