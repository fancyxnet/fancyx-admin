using Dapper;
using Fancyx.Admin.EfCore.Entities.Feedback;
using Fancyx.Admin.EfCore.Models;
using Cracker.AspNetCore.AutoInject;
using Cracker.EfCore;
using Cracker.EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Repositories
{
    [DependencyInject(AsSelf = true)]
    public class TicketRepository : BaseRepository<Ticket>
    {
        public TicketRepository(DbContext context) : base(context)
        {
        }

        public Task<EntityPaged<TicketItem>> QueryListAsync(int current, int pageSize)
        {
            var sql = @"select t.id,t.title,t.content,t.status,t.rating,rating_comment as RatingComment,t.creation_time as CreationTime as AssignedUserId,
                        u1.nick_name as SenderNickName,u2.nick_name as AssignedNickName,IF(t.assigned_user_id,true,false) as IsReply
                        from ticket inner join user u1 on t.user_id = u1.id
                        left join user u2 on t.assigned_user_id = u2.id
                        order by t.creation_time desc";
            return Connection.QueryListFromSqlAsync<TicketItem>(current, pageSize, sql);
        }

        public async Task<List<TicketReplyInfo>> QueryReplyListAsync(long ticketId)
        {
            var sql = @"select tr.creation_time as CreationTime,tr.content,tr.is_assigned_user as IsAssignedUser,u.nick_name as AssignedNickName
                        from ticket_reply tr inner join user u on tr.sender_id=u.id
                        order by tr.creation_time desc";
            return (await Connection.QueryAsync<TicketReplyInfo>(sql, new { id = ticketId })).ToList();
        }
    }
}