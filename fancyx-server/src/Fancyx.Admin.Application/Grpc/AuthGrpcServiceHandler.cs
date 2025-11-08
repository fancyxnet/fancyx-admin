using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using Fancyx.Internal.Grpc.System;
using Fancyx.Shared.Keys;
using Grpc.Core;
using StackExchange.Redis;

namespace Fancyx.Admin.Application.Grpc
{
    public class AuthGrpcServiceHandler : Auth.AuthBase
    {
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IDatabase _redisClient;

        public AuthGrpcServiceHandler(IdentitySharedService identitySharedService, IRepository<Tenant> tenantRepository, IDatabase redisClient)
        {
            _identitySharedService = identitySharedService;
            _tenantRepository = tenantRepository;
            _redisClient = redisClient;
        }

        public async override Task<GetUserPermissionRes> GetUserPermission(GetUserPermissionReq request, ServerCallContext context)
        {
            var permission = await _identitySharedService.GetUserPermissionAsync(request.UserId);
            var res = new GetUserPermissionRes();
            res.Auths.AddRange(permission.Auths);
            return res;
        }

        public async override Task<ExistTenantRes> ExistTenant(ExistTenantReq request, ServerCallContext context)
        {
            var tenantId = request.TenantId;
            if (await _redisClient.KeyExistsAsync(SystemCacheKey.AllTenant))
            {
                return new ExistTenantRes
                {
                    IsExist = await _redisClient.HashExistsAsync(SystemCacheKey.AllTenant, tenantId)
                };
            }
            var tenants = await _tenantRepository.GetQueryable().SelectToListAsync(x => new { TenantId = x.Id, x.Name });
            var map = tenants.ToDictionary(k => k.TenantId, v => v.Name);
            if (map.Count == 0) return new ExistTenantRes { IsExist = false };

            await _redisClient.HashSetAsync(SystemCacheKey.AllTenant, map.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
            return new ExistTenantRes { IsExist = map.ContainsKey(tenantId) };
        }
    }
}
