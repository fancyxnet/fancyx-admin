using Cracker.EfCore.BaseEntity;
using Cracker.IdentityServer.Abstractions;

namespace Fancyx.Admin.EfCore.Entities.Feedback
{
    /// <summary>
    /// 工单回复
    /// </summary>
    public class TicketReply : CreationEntity<long>, ITenant
    {
        public long TicketId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; } = null!;
        public string? TenantId { get; set; }
    }
}