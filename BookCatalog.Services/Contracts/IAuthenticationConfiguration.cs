using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookCatalog.Services.Contracts;

public interface IAuthenticationConfiguration
{
    string TokenIssuer { get; }
    
    TimeSpan AccessTokenLifeTime { get; }
    TimeSpan RefreshTokenLifeTime { get; }

    SymmetricSecurityKey GetSymmetricSecurityKey();
}