using System.Security.Claims;

using Fancyx.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Fancyx.Core.Authorization
{
    public class CurrentUser : ICurrentUser
    {
        public long? Id => _id;

        public string? UserName => _userName;

        private readonly long? _id;
        private readonly string? _userName;
        private readonly IEnumerable<Claim>? _claims;

        public CurrentUser(long? id, string? userName, IEnumerable<Claim>? claims)
        {
            _id = id;
            _userName = userName;
            _claims = claims;
        }

        public Claim FindClaim(string type)
        {
            return _claims?.FirstOrDefault(x => x.Type == type) ?? new Claim(type, string.Empty);
        }

        public static ICurrentUser Default()
        {
            return new CurrentUser(null, null, null);
        }

        public IEnumerable<Claim> GetClaims()
        {
            return _claims ?? [];
        }

        public bool IsInRoles(params string[] roles)
        {
            if (roles.Length == 0) return false;
            var hasRoles = this.FindClaim(ClaimTypes.Role).Value;
            return roles.All(x => hasRoles.Contains(x));
        }

        public static ICurrentUser Parse(HttpContext? context)
        {
            return context?.Features.Get<CurrentUser>() ?? Default();
        }
    }
}