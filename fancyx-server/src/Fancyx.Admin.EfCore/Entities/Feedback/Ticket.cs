using Fancyx.Admin.EfCore.Enums;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;

namespace Fancyx.Admin.EfCore.Entities.Feedback
{
    /// <summary>
    /// 工单
    /// </summary>
    public class Ticket : FullAuditedEntity<long>, ITenant
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public TicketStatus Status { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; }
        public string? RatingComment { get; set; }
        public long? AssignedUserId { get; set; }
        public string? TenantId { get; set; }
    }
}