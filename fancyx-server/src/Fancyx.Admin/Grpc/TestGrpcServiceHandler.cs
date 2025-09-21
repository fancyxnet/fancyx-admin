using Fancyx.Internal.Grpc;
using Grpc.Core;

namespace Fancyx.Admin.Grpc
{
    public class TestGrpcServiceHandler : Test.TestBase
    {
        public override Task<GetTestInfoResponse> GetTestInfo(GetTestInfoRequest request, ServerCallContext context)
        {
            if (request.Id == 1)
            {
                return Task.FromResult(new GetTestInfoResponse
                {
                    Id = 1,
                    Name = "Test Name 1"
                });
            }
            else
            {
                return Task.FromResult(new GetTestInfoResponse
                {
                    Id = 2,
                    Name = "Test Name 2"
                });
            }
        }
    }
}