using AutoMapper;
using Fancyx.Admin.EfCore.Entities.Job;
using Fancyx.Admin.IService.System.Dtos;

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