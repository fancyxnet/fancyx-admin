using Fancyx.Core.AutoInject;
using Fancyx.Job;
using Fancyx.Payment.Enums;
using Fancyx.Repository;
using Fancyx.Repository.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Quartz;
using RedLockNet.SERedis;

namespace Fancyx.Payment.Jobs
{
    [DenpendencyInject(AsSelf = true)]
    [JobKey("PaymentOrderJob")]
    public class PaymentOrderJob : IJob
    {
        private readonly RedLockFactory _redLockFactory;
        private readonly IRepository<PaymentOrder> _paymentOrderRepository;

        public PaymentOrderJob(RedLockFactory redLockFactory, IRepository<PaymentOrder> paymentOrderRepository)
        {
            _redLockFactory = redLockFactory;
            _paymentOrderRepository = paymentOrderRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var expiry = TimeSpan.FromSeconds(30);
            var wait = TimeSpan.FromSeconds(10);
            var retry = TimeSpan.FromSeconds(1);

            using var redLock = await _redLockFactory.CreateLockAsync(nameof(PaymentOrderJob), expiry, wait, retry);
            if (!redLock.IsAcquired) return;

            var now = DateTime.Now;
            await _paymentOrderRepository.Where(x => now >= x.InitiationTime.AddMinutes(15) && x.PayStatus == PayStatus.Processing.GetStatus())
                .ExecuteUpdateAsync(x => x.SetProperty(s => s.PayStatus, PayStatus.Timeout.GetStatus())
                .SetProperty(s => s.TimeoutTime, now));
        }
    }
}