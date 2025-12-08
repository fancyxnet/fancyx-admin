using AutoMapper;
using Fancyx.Admin.Application.IService.Feedback.Models;
using Fancyx.Admin.EfCore.Models;

namespace Fancyx.Admin.Application.Profiles
{
    public class FeedbackAutoMapperProfile : Profile
    {
        public FeedbackAutoMapperProfile()
        {
            CreateMap<TicketReplyInfo, TicketReplyListDto>(MemberList.None);
        }
    }
}