using AutoMapper;

using Fancyx.Admin.IService.System.Dtos;
using Fancyx.DataAccess.Entities.Job;

namespace Fancyx.Admin.Profiles
{
    public class JobAutoMapperProfile : Profile
    {
        public JobAutoMapperProfile()
        {
            CreateMap<TaskExecutionLog, TaskExecutionLogListDto>();
        }
    }
}