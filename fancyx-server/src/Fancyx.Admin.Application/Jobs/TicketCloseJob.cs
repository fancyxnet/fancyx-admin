using Coravel.Invocable;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.EfCore.Repositories;
using Cracker.AspNetCore.AutoInject;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Jobs
{
    [DependencyInject(AsSelf = true)]
    public class TicketCloseJob : IInvocable
    {
        private readonly TicketRepository _ticketRepository;

        public TicketCloseJob(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Invoke()
        {
            var expiry = TimeSpan.FromSeconds(30);
            var wait = TimeSpan.FromSeconds(10);
            var retry = TimeSpan.FromSeconds(1);

            //TODO:
            //using var redLock = await _redLockFactory.CreateLockAsync(nameof(NotificationJob), expiry, wait, retry);
            if (false) return;

            await _ticketRepository.Where(x => x.Status == TicketStatus.Processing && x.CreationTime > x.CreationTime.AddDays(7))
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Status, TicketStatus.Closed)
                .SetProperty(e => e.LastModificationTime, DateTime.Now));
        }
    }
}