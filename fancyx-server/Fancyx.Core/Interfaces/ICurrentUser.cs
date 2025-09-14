using System.Security.Claims;

namespace Fancyx.Core.Interfaces
{
    public interface ICurrentUser
    {
        Guid? Id { get; }

        string? UserName { get; }

        Claim FindClaim(string type);

        IEnumerable<Claim> GetClaims();

        bool IsInRoles(params string[] roles);
    }
}