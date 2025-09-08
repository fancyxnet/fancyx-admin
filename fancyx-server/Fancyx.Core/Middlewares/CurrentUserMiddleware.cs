using Fancyx.Core.Authorization;
using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace Fancyx.Core.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? subId = context.User.FindFirst(x => x.Type == "sub" || x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(subId))
            {
                await next(context);
                return;
            }
            string? userName = context.User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
            context.Features.Set(new CurrentUser(Guid.Parse(subId), userName, context.User.Claims));
            UserManager.SetCurrent(Guid.Parse(subId));

            await next(context);
        }
    }
}