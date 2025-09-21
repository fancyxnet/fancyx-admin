using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers
{
    [ApiController]
    [Route("/private-api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public AppResponse<string> Hello()
        {
            return Result.Data("hello");
        }
    }
}
