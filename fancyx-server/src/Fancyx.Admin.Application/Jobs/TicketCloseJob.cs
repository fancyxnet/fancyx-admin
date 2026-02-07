using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.EfCore.Repositories;
using Cracker.AspNetCore.AutoInject;
using Microsoft.EntityFrameworkCore;
using Cracker.Scheduler;
using Quartz;

namespace Fancyx.Admin.Application.Jobs
{
    [DependencyInject(AsSelf = true)]
    [DisallowConcurrentExecution]
    public class TicketCloseJob : JobBase
    {
        private readonly TicketRepository _ticketRepository;

        public TicketCloseJob(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async override Task Invoke(JobRunningContext ctx)
        {
            await _ticketRepository.Where(x => x.Status == TicketStatus.Processing && x.CreationTime > x.CreationTime.AddDays(7))
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Status, TicketStatus.Closed)
                .SetProperty(e => e.LastModificationTime, DateTime.Now));
        }
    }
}