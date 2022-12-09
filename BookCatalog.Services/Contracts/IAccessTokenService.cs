using System.Security.Claims;

namespace BookCatalog.Services.Contracts;

public interface IAccessTokenService
{
    string Create(IEnumerable<Claim> claims, DateTime utcNow);
    ClaimsPrincipal GetPrincipal(string token);
}