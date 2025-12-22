using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Cache;
using Fancyx.EfCore;
using Fancyx.Internal.Grpc.System;
using Fancyx.Shared.Keys;

using Grpc.Core;

using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

namespace Fancyx.Admin.Application.Grpc
{
    public class AuthGrpcServiceHandler : Auth.AuthBase
    {
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICacheClient _cache;

        public AuthGrpcServiceHandler(IdentitySharedService identitySharedService, IRepository<Tenant> tenantRepository, ICacheClient cache)
        {
            _identitySharedService = identitySharedService;
            _tenantRepository = tenantRepository;
            _cache = cache;
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
            if (await _cache.KeyExistsAsync(SystemCacheKey.AllTenant))
            {
                return new ExistTenantRes
                {
                    IsExist = await _cache.HashExistsAsync(SystemCacheKey.AllTenant, tenantId)
                };
            }
            var map = await _tenantRepository.Where(x => x.IsEnabled).ToDictionaryAsync(k => k.Id, v => v.Name);
            if (map.Count == 0) return new ExistTenantRes { IsExist = false };

            await _cache.HashSetAsync(SystemCacheKey.AllTenant, map.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
            return new ExistTenantRes { IsExist = map.ContainsKey(tenantId) };
        }

        public async override Task<GetTenantByDomainRes> GetTenantByDomain(GetTenantByDomainReq request, ServerCallContext context)
        {
            if (await _cache.KeyExistsAsync(SystemCacheKey.TenantDomains))
            {
                return new GetTenantByDomainRes
                {
                    TenantId = await _cache.HashGetAsync(SystemCacheKey.TenantDomains, request.Domain)
                };
            }
            var map = await _tenantRepository.Where(x => x.IsEnabled).ToDictionaryAsync(x => x.Domain, k => k.Id);
            if (map.Count == 0) return new GetTenantByDomainRes { TenantId = null };

            await _cache.HashSetAsync(SystemCacheKey.TenantDomains, map.Select(x => new HashEntry(x.Key, x.Value)).ToArray());
            return new GetTenantByDomainRes { TenantId = map.TryGetValue(request.Domain, out var tenantId) ? tenantId : null };
        }
    }
}
