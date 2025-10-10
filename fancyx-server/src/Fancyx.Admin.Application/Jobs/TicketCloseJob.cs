using Coravel.Invocable;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.EfCore.Repositories;
using Fancyx.Core.AutoInject;
using Microsoft.EntityFrameworkCore;
using RedLockNet.SERedis;

namespace Fancyx.Admin.Application.Jobs
{
    [DenpendencyInject(AsSelf = true)]
    public class TicketCloseJob : IInvocable
    {
        private readonly TicketRepository _ticketRepository;
        private readonly RedLockFactory _redLockFactory;

        public TicketCloseJob(TicketRepository ticketRepository, RedLockFactory redLockFactory)
        {
            _ticketRepository = ticketRepository;
            _redLockFactory = redLockFactory;
        }

        public async Task Invoke()
        {
            var expiry = TimeSpan.FromSeconds(30);
            var wait = TimeSpan.FromSeconds(10);
            var retry = TimeSpan.FromSeconds(1);

            using var redLock = await _redLockFactory.CreateLockAsync(nameof(NotificationJob), expiry, wait, retry);
            if (!redLock.IsAcquired) return;

            await _ticketRepository.Where(x => x.Status == TicketStatus.Processing && x.CreationTime > x.CreationTime.AddDays(7))
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Status, TicketStatus.Closed)
                .SetProperty(e => e.LastModificationTime, DateTime.Now));
        }
    }
}