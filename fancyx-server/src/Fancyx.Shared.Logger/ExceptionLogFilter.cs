using Cracker.IdentityServer;
using DotNetCore.CAP;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Logger.Message;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fancyx.Shared.Logger
{
    /// <summary>
    /// 异常日志收集（异常会继续往下传递，不标记已处理）
    /// </summary>
    public sealed class ExceptionLogFilter : IAsyncExceptionFilter
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<ExceptionLogFilter> _logger;
        private static Type[] IgnoreExceptionTypes = [typeof(BusinessException), typeof(EntityNotFoundException)];

        public ExceptionLogFilter(ICapPublisher capPublisher, ILogger<ExceptionLogFilter> logger)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            try
            {
                var exceptionType = context.Exception.GetType();
                if (IgnoreExceptionTypes.Contains(exceptionType))
                {
                    // 忽略指定的异常类型
                    return;
                }

                var msg = new ExceptionLogMessage()
                {
                    ExceptionType = exceptionType.FullName ?? exceptionType.Name,
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace ?? string.Empty,
                    InnerException = context.Exception.InnerException?.Message,
                    RequestPath = context.HttpContext.Request.Path,
                    RequestMethod = context.HttpContext.Request.Method,
                    TraceId = Activity.Current?.TraceId.ToString(),
                    Ip = HttpContextUtils.GetIp(context.HttpContext),
                    UserAgent = context.HttpContext.Request.Headers.UserAgent
                };
                var currentUser = CurrentUser.Parse(context.HttpContext);
                var currentTenant = CurrentTenant.Parse(context.HttpContext);
                msg.UserId = currentUser?.Id;
                msg.UserName = currentUser?.UserName;
                msg.TenantId = currentTenant?.TenantId;

                await _capPublisher.PublishAsync(LoggerEventBusTopicConsts.EXCEPTION_LOG_EVENT, msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异常日志收集失败");
            }
        }
    }
}