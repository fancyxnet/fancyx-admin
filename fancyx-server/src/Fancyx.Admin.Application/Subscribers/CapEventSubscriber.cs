using DotNetCore.CAP;
using Fancyx.Shared.Consts;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.SnowflakeId;

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
            log.Id = IdGenerater.Instance.NextId();
            await _context.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}