using Fancyx.Core.Interfaces;
using Fancyx.Internal.Grpc.System;

namespace Fancyx.Shared.WebApi
{
    public class TenantChecker : ITenantChecker, ISingletonDependency
    {
        public TenantChecker(Auth.AuthClient authClient)
        {
            AuthClient = authClient;
        }

        public Auth.AuthClient AuthClient { get; }

        public async Task<bool> ExistTenantAsync(string tenantId)
        {
            var res = await AuthClient.ExistTenantAsync(new ExistTenantReq { TenantId = tenantId });
            return res.IsExist;
        }
    }
}