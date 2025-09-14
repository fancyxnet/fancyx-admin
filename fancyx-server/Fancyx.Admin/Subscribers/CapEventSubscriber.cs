using DotNetCore.CAP;
using Fancyx.Shared.Consts;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Log;

namespace Fancyx.Admin.Subscribers
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