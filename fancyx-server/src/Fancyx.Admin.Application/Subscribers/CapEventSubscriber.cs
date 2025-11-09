using DotNetCore.CAP;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Shared.Consts;
using Fancyx.SnowflakeId;

namespace Fancyx.Admin.Application.Subscribers
{
    public class CapEventSubscriber : ICapSubscribe
    {
        private readonly FancyxDbContext _context;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IdentitySharedService _identitySharedService;
        private readonly ICapPublisher _capPublisher;

        public CapEventSubscriber(FancyxDbContext context, IUserService userService, IRoleService roleService, IdentitySharedService identitySharedService, ICapPublisher capPublisher)
        {
            _context = context;
            _userService = userService;
            _roleService = roleService;
            _identitySharedService = identitySharedService;
            _capPublisher = capPublisher;
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