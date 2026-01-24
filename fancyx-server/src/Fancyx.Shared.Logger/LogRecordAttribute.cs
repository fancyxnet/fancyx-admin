using DotNetCore.CAP;

using Fancyx.Core.Authorization;
using Fancyx.Core.AutoInject;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Logger.Message;
using Fancyx.Utils;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace Fancyx.Shared.Logger
{
    /// <summary>
    /// 操作日志记录，加在业务方法上（只适合异步方法）
    /// </summary>
    public sealed class LogRecordAttribute : AopAttributeBase
    {
        public string Type { get; init; }
        public string SubType { get; init; }
        public string BizNo { get; init; }
        public string Content { get; init; }

        public LogRecordAttribute(string type, string subType, string bizNo, string content) : base(true)
        {
            Type = type;
            SubType = subType;
            BizNo = bizNo;
            Content = content;
        }

        public override async Task OnAfterAsync()
        {
            var eventBus = ServiceProvider.GetService<ICapPublisher>();
            if (eventBus == null) return;
            var msg = new LogRecordMessage();
            try
            {
                var map = LogRecordContext.GetVariables().ToDictionary();
                msg.Type = TemplateUtils.Render(Type!, map);
                msg.SubType = TemplateUtils.Render(SubType!, map);
                msg.BizNo = TemplateUtils.Render(BizNo!, map);
                msg.Content = TemplateUtils.Render(Content!, map);
                msg.CreationTime = DateTime.Now;
                var httpContext = ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
                if (httpContext != null)
                {
                    var currentUser = CurrentUser.Parse(httpContext);
                    var currentTenant = CurrentTenant.Parse(httpContext);
                    msg.UserId = currentUser.Id;
                    msg.UserName = currentUser.UserName;
                    msg.TenantId = currentTenant.TenantId;
                    msg.Ip = HttpUtils.GetIp(httpContext);
                    msg.UserAgent = httpContext.Request.Headers.UserAgent;
                    msg.TraceId = Activity.Current?.TraceId.ToString();
                }
                LogRecordContext.Dispose();
                await eventBus.PublishAsync(LoggerEventBusTopicConsts.LOG_RECORD_EVENT, msg);
            }
            catch (Exception ex)
            {
                var logger = ServiceProvider.GetService<ILogger<LogRecordAttribute>>();
                logger?.LogError(ex, "操作日志[{type}]发布异常", msg.Type);
            }
        }

        public override Task OnBeforeAsync()
        {
            LogRecordContext.Init();
            return Task.CompletedTask;
        }

        public override Task OnExceptionAsync()
        {
            LogRecordContext.Dispose();
            return base.OnExceptionAsync();
        }
    }
}