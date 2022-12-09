using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookCatalog.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace BookCatalog.Services.Implementations;

public class AccessTokenService : IAccessTokenService
{
    private readonly IAuthenticationConfiguration _authenticationConfiguration;

    public AccessTokenService(IAuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }

    public string Create(IEnumerable<Claim> claims, DateTime utcNow)
    {
        var signingCredentials = new SigningCredentials(
            key: _authenticationConfiguration.GetSymmetricSecurityKey(),
            algorithm: SecurityAlgorithms.HmacSha256
        );

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _authenticationConfiguration.TokenIssuer,
            claims: claims,
            notBefore: utcNow,
            expires: utcNow.Add(_authenticationConfiguration.AccessTokenLifeTime),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public ClaimsPrincipal GetPrincipal(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _authenticationConfiguration.TokenIssuer,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _authenticationConfiguration.GetSymmetricSecurityKey(),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        ClaimsPrincipal claimsPrincipal =
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken)
        {
            throw new SecurityTokenException();
        }

        if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.Ordinal))
        {
            throw new SecurityTokenException();
        }

        return claimsPrincipal;
    }
}