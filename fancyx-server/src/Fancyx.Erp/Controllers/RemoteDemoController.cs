using Fancyx.Erp.Remote;
using Fancyx.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteDemoController : ControllerBase
    {
        private readonly ITestApi _testApi;

        public RemoteDemoController(ITestApi testApi)
        {
            _testApi = testApi;
        }

        [HttpGet]
        public async Task<AppResponse<string>> HelloAsync()
        {
            return await _testApi.Hello();
        }
    }
}
