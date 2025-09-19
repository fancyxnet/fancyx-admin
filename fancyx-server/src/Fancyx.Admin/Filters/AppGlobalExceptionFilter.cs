using Fancyx.Shared.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fancyx.Admin.Filters
{
    public class AppGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<AppGlobalExceptionFilter> _logger;

        public AppGlobalExceptionFilter(ILogger<AppGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;

            var errMsg = context.Exception.Message;
            var result = new AppResponse<bool>(ErrorCode.Fail, errMsg).SetData(false);

            if (context.Exception is BusinessException businessException && !string.IsNullOrEmpty(businessException.Code))
            {
                result.Code = businessException.Code;
            }
            else if (context.Exception is EntityNotFoundException entityNotFoundException)
            {
                result.Message = !string.IsNullOrEmpty(entityNotFoundException.Message) ? entityNotFoundException.Message : "数据不存在";
            }

            context.Result = new ObjectResult(result);
            context.ExceptionHandled = true;

            if (context.Exception is BusinessException) return;

            _logger.LogError(context.Exception, "全局捕获异常");
        }
    }
}