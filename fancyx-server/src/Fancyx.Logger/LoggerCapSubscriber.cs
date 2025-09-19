using DotNetCore.CAP;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.Log;
using Fancyx.Logger.Consts;
using Fancyx.Logger.Message;
using Fancyx.Utils;

namespace Fancyx.Logger
{
    public class LoggerCapSubscriber : ICapSubscribe
    {
        private readonly FancyxDbContext _context;

        public LoggerCapSubscriber(FancyxDbContext context)
        {
            _context = context;
        }

        [CapSubscribe(EventBusTopicConsts.LOG_RECORD_EVENT)]
        public async Task LogRecord(LogRecordMessage message)
        {
            var entity = new LogRecord
            {
                Type = message.Type,
                SubType = message.SubType,
                BizNo = message.BizNo,
                Content = message.Content,
                Ip = message.Ip,
                Browser = HttpUtils.GetBrowserByUA(message.UserAgent),
                UserId = message.UserId,
                UserName = message.UserName,
                TraceId = message.TraceId,
                CreatorId = message.UserId,
                TenantId = message.TenantId,
                CreationTime = message.CreationTime
            };

            await _context.SingleInsertAsync(entity);
        }

        [CapSubscribe(EventBusTopicConsts.API_ACCESS_LOG_EVENT)]
        public async Task ApiAccessLog(ApiAccessLogMessage message)
        {
            var entity = new ApiAccessLog
            {
                Path = message.Path,
                Method = message.Method,
                RequestTime = message.RequestTime,
                OperateType = message.OperateType,
                OperateName = message.OperateName,
                QueryString = message.QueryString,
                RequestBody = message.RequestBody,
                ResponseBody = message.ResponseBody,
                ResponseTime = message.ResponseTime,
                Duration = message.Duration,
                Ip = message.Ip,
                Browser = HttpUtils.GetBrowserByUA(message.UserAgent),
                UserId = message.UserId,
                UserName = message.UserName,
                TraceId = message.TraceId,
                CreatorId = message.UserId,
                TenantId = message.TenantId,
                CreationTime = DateTime.Now
            };

            await _context.SingleInsertAsync(entity);
        }

        [CapSubscribe(EventBusTopicConsts.EXCEPTION_LOG_EVENT)]
        public async Task ExceptionLog(ExceptionLogMessage message)
        {
            var entity = new ExceptionLog()
            {
                RequestPath = message.RequestPath,
                RequestMethod = message.RequestMethod,
                ExceptionType = message.ExceptionType,
                Message = message.Message,
                StackTrace = message.StackTrace,
                InnerException = message.InnerException,
                Ip = message.Ip,
                Browser = HttpUtils.GetBrowserByUA(message.UserAgent),
                UserId = message.UserId,
                UserName = message.UserName,
                TraceId = message.TraceId,
                CreatorId = message.UserId,
                TenantId = message.TenantId,
                CreationTime = DateTime.Now
            };

            await _context.SingleInsertAsync(entity);
        }
    }
}