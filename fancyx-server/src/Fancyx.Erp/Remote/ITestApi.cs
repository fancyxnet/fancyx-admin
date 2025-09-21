using Fancyx.Shared.Models;

using Refit;

namespace Fancyx.Erp.Remote
{
    public interface ITestApi
    {
        [Get("/private-api/test/hello")]
        Task<AppResponse<string>> Hello();
    }
}
