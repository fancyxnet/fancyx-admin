using DotNetCore.CAP;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Core.Authorization;
using Fancyx.Logger.Consts;
using Fancyx.Logger.Message;
using Fancyx.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Fancyx.Logger
{
    public sealed class ApiAccessLogAttribute : ActionFilterAttribute
    {
        public ApiAccessLogAttribute(string? operateName = default, OperateType[]? operateType = default, bool requestEnable = true, bool reponseEnable = false)
        {
            OperateName = operateName;
            OperateType = operateType;
            RequestEnable = requestEnable;
            ResponseEnable = reponseEnable;
        }

        public bool RequestEnable { get; init; } = true;

        public bool ResponseEnable { get; init; } = true;

        public OperateType[]? OperateType { get; init; }

        public string? OperateName { get; init; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var msg = new ApiAccessLogMessage()
                {
                    Path = context.HttpContext.Request.Path,
                    Method = context.HttpContext.Request.Method,
                    RequestTime = DateTime.Now,
                    OperateType = OperateType,
                    OperateName = OperateName,
                    TraceId = Activity.Current?.TraceId.ToString(),
                    Ip = HttpUtils.GetIp(context.HttpContext),
                    UserAgent = context.HttpContext.Request.Headers.UserAgent,
                };
                if (RequestEnable)
                {
                    msg.RequestBody = JsonConvert.SerializeObject(context.ActionArguments);
                    msg.QueryString = context.HttpContext.Request.QueryString.Value;
                }
                var currentUser = CurrentUser.Parse(context.HttpContext);
                var currentTenant = CurrentTenant.Parse(context.HttpContext);
                msg.UserId = currentUser?.Id;
                msg.UserName = currentUser?.UserName;
                msg.TenantId = currentTenant?.TenantId;

                if (ResponseEnable)
                {
                    context.HttpContext.Items.Add("ApiAccessLogMessage", msg);
                }
                else
                {
                    await PushAsync(context.HttpContext, msg);
                }
            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ApiAccessLogAttribute>>();
                logger?.LogError(ex, "API访问日志收集失败");
            }
            finally
            {
                await next();
            }
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (ResponseEnable)
            {
                bool hasMsg = context.HttpContext.Items.TryGetValue("ApiAccessLogMessage", out var msg);
                if (hasMsg && msg is ApiAccessLogMessage apiAccessLogMsg)
                {
                    var now = DateTime.Now;
                    apiAccessLogMsg.ResponseTime = now;
                    apiAccessLogMsg.Duration = (long)Math.Round(now.Subtract(apiAccessLogMsg.RequestTime).TotalMilliseconds);
                    apiAccessLogMsg.ResponseBody = StringUtils.CutOff(JsonConvert.SerializeObject(context.Result), 500);
                    await PushAsync(context.HttpContext, apiAccessLogMsg);
                }
            }
            await next();
        }

        private async Task PushAsync(HttpContext httpContext, ApiAccessLogMessage msg)
        {
            var capPublisher = httpContext.RequestServices.GetService<ICapPublisher>();
            if (capPublisher != null)
            {
                await capPublisher.PublishAsync(EventBusTopicConsts.API_ACCESS_LOG_EVENT, msg);
            }
        }
    }
}