using Fancyx.EfCore.BaseEntity;

namespace Fancyx.Admin.EfCore.Entities.Feedback
{
    /// <summary>
    /// 工单回复
    /// </summary>
    public class TicketReply : CreationEntity
    {
        public long TicketId { get; set; }
        public long SenderId { get; set; }
        public long Content { get; set; }
    }
}