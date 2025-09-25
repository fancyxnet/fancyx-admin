using DotNetCore.CAP;
using Fancyx.Shared.Consts;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;

namespace Fancyx.Admin.Application.Subscribers
{
    public class CapEventSubscriber : ICapSubscribe
    {
        private readonly FancyxDbContext _context;

        public CapEventSubscriber(FancyxDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(AdminEventBusTopicConsts.LoginLogEvent)]
        public async Task WriteLoginLog(LoginLog log)
        {
            log.CreationTime = DateTime.Now;
            await _context.SingleInsertAsync(log);
        }
    }
}