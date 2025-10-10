using Fancyx.Shared.Models;

using Refit;

namespace Fancyx.Erp.Application.Remote
{
    public interface ITestApi
    {
        [Get("/private-api/test/hello")]
        Task<AppResponse<string>> Hello();
    }
}
