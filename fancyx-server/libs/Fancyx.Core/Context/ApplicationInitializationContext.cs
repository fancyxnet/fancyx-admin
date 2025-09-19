using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace Fancyx.Core.Context
{
    public class ApplicationInitializationContext
    {
        internal ApplicationInitializationContext(WebApplication webApplication)
        {
            Application = webApplication;
        }

        private WebApplication Application { get; }

        public IApplicationBuilder GetApplicationBuilder()
        {
            return Application;
        }

        public IWebHostEnvironment Environment => Application.Environment;

        public IEndpointRouteBuilder Endpoint => Application;

        public IServiceProvider ServiceProvider => Application.Services;
    }
}