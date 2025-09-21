using Fancyx.Erp.Remote;
using Fancyx.Internal.Grpc;
using Fancyx.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteDemoController : ControllerBase
    {
        private readonly ITestApi _testApi;
        private readonly Test.TestClient _testClient;

        public RemoteDemoController(ITestApi testApi, Test.TestClient testClient)
        {
            _testApi = testApi;
            _testClient = testClient;
        }

        [HttpGet("Hello_Http")]
        public async Task<AppResponse<string>> HelloAsync()
        {
            return await _testApi.Hello();
        }

        [HttpGet("GetTestInfo_Grpc")]
        public async Task<AppResponse<GetTestInfoResponse>> GetTestInfoAsync(int id)
        {
            var request = new GetTestInfoRequest { Id = id };
            var response = await _testClient.GetTestInfoAsync(request);
            return Result.Data(response);
        }
    }
}
