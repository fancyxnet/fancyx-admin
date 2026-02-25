using DotNetCore.CAP;
using Cracker.EfCore;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger.Entities;
using Fancyx.Shared.Logger.Message;
using Cracker.Utils;

namespace Fancyx.Logger
{
    public class LoggerCapSubscriber : ICapSubscribe
    {
        private readonly IRepository<LogRecord> _logRecordRepository;
        private readonly IRepository<ApiAccessLog> _apiAccessLogRepository;
        private readonly IRepository<ExceptionLog> _exceptionLogRepository;

        public LoggerCapSubscriber(IRepository<LogRecord> logRecordRepository, IRepository<ApiAccessLog> apiAccessLogRepository
            , IRepository<ExceptionLog> exceptionLogRepository)
        {
            _logRecordRepository = logRecordRepository;
            _apiAccessLogRepository = apiAccessLogRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        [CapSubscribe(LoggerEventBusTopicConsts.LOG_RECORD_EVENT)]
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

            await _logRecordRepository.InsertAsync(entity);
        }

        [CapSubscribe(LoggerEventBusTopicConsts.API_ACCESS_LOG_EVENT)]
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

            await _apiAccessLogRepository.InsertAsync(entity);
        }

        [CapSubscribe(LoggerEventBusTopicConsts.EXCEPTION_LOG_EVENT)]
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

            await _exceptionLogRepository.InsertAsync(entity);
        }
    }
}