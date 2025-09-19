using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fancyx.Admin.Filters
{
    public class HttpRequestValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = context.ModelState.Where(x => x.Value != null && x.Value.Errors != null && x.Value.Errors.Count > 0).FirstOrDefault().Value!.Errors.FirstOrDefault();
                if (error != null)
                {
                    context.Result = new ObjectResult(Result.Fail(error.ErrorMessage));
                    return;
                }
            }
        }
    }
}