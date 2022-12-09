using System.Text;
using BookCatalog.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace BookCatalog.Api.Configurations;

public class AuthenticationConfiguration : IAuthenticationConfiguration
{
    public string TokenIssuer => _configuration.GetSection(SectionName)[nameof(TokenIssuer)];
    public TimeSpan AccessTokenLifeTime => TimeSpan.FromSeconds(AccessTokenLifeTimeSeconds);
    public TimeSpan RefreshTokenLifeTime => TimeSpan.FromSeconds(RefreshTokenLifeTimeSeconds);

    private int AccessTokenLifeTimeSeconds =>
        int.Parse(_configuration.GetSection(SectionName)[nameof(AccessTokenLifeTimeSeconds)]);

    private int RefreshTokenLifeTimeSeconds =>
        int.Parse(_configuration.GetSection(SectionName)[nameof(RefreshTokenLifeTimeSeconds)]);

    private string SecurityKey => _configuration.GetSection(SectionName)[nameof(SecurityKey)];

    private const string SectionName = "AuthenticationOptions";
    private readonly IConfiguration _configuration;

    public AuthenticationConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));
    }
}