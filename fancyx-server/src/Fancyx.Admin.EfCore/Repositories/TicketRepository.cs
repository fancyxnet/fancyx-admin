using Fancyx.Admin.EfCore.Entities.Feedback;
using Fancyx.Admin.EfCore.Models;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Repositories
{
    [DenpendencyInject(AsSelf = true)]
    public class TicketRepository : BaseRepository<Ticket>
    {
        public TicketRepository(DbContext context, ICurrentUser currentUser) : base(context, currentUser)
        {
        }

        public Task<EntityPaged<TicketItem>> QueryListAsync(int current, int pageSize)
        {
            // TODO:
            var sql = @"";
            return Connection.QueryListFromSqlAsync<TicketItem>(current, pageSize, sql);
        }
    }
}