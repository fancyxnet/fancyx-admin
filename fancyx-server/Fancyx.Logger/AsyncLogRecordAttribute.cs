using DotNetCore.CAP;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Utils;
using Fancyx.Logger.Consts;
using Fancyx.Logger.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fancyx.Logger
{
    /// <summary>
    /// 操作日志记录，加在业务方法上（只适合异步方法）
    /// </summary>
    public sealed class AsyncLogRecordAttribute : AsyncAopAttributeBase
    {
        public string Type { get; init; }
        public string SubType { get; init; }
        public string BizNo { get; init; }
        public string Content { get; init; }

        public AsyncLogRecordAttribute(string type, string subType, string bizNo, string content) : base(true)
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
                    var currentUser = RequestUtils.ResolveUser(httpContext);
                    var currentTenant = RequestUtils.ResolveTenant(httpContext);
                    msg.UserId = currentUser.Id;
                    msg.UserName = currentUser.UserName;
                    msg.TenantId = currentTenant.TenantId;
                    msg.Ip = RequestUtils.GetIp(httpContext);
                    msg.UserAgent = RequestUtils.GetUserAgent(httpContext);
                    msg.TraceId = Activity.Current?.TraceId.ToString();
                }
                LogRecordContext.Dispose();
                await eventBus.PublishAsync(EventBusTopicConsts.LOG_RECORD_EVENT, msg);
            }
            catch (Exception ex)
            {
                var logger = ServiceProvider.GetService<ILogger<AsyncLogRecordAttribute>>();
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