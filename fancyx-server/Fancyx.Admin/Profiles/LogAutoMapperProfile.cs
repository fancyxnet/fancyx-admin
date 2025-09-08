using AutoMapper;

using Fancyx.Admin.IService.Monitor.Dtos;
using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Admin.IService.System.LogManagement.Dtos;
using Fancyx.DataAccess.Entities.Log;
using Fancyx.DataAccess.Entities.Payment;

namespace Fancyx.Admin.Profiles
{
    public class LogAutoMapperProfile: Profile
    {
        public LogAutoMapperProfile()
        {
            CreateMap<ApiAccessLog, ApiAccessLogListDto>();
            CreateMap<ExceptionLog, ExceptionLogListDto>();
            CreateMap<LogRecord, BusinessLogListDto>();
            CreateMap<LoginLog, LoginLogListDto>();
            CreateMap<LoginLog, LoginLogListDto>();
        }
    }
}
