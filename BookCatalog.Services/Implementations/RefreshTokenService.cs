using System.Security.Cryptography;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IAuthenticationConfiguration _authenticationConfiguration;

    public RefreshTokenService(IAuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }

    public UserRefreshTokenDto Create(DateTime utcNow)
    {
        var userRefreshTokenDto = new UserRefreshTokenDto
        {
            ExpirationDateTime = utcNow.Add(_authenticationConfiguration.RefreshTokenLifeTime),
            Token = GenerateRefreshToken()
        };

        return userRefreshTokenDto;
    }

    public UserRefreshTokenDto Create(UserRefreshTokenRecord userRefreshTokenRecord)
    {
        return new UserRefreshTokenDto
        {
            Token = userRefreshTokenRecord.Token,
            ExpirationDateTime = userRefreshTokenRecord.ExpirationDateTime
        };
    }

    public UserRefreshTokenRecord CreateRecord(UserRefreshTokenDto refreshToken)
    {
        return new UserRefreshTokenRecord
        {
            Token = refreshToken.Token,
            ExpirationDateTime = refreshToken.ExpirationDateTime
        };
    }

    public void UpdateRecord(UserRefreshTokenRecord userRefreshTokenRecord, UserRefreshTokenDto userRefreshTokenDto)
    {
        userRefreshTokenRecord.Token = userRefreshTokenDto.Token;
        userRefreshTokenRecord.ExpirationDateTime = userRefreshTokenDto.ExpirationDateTime;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumberBytes = new byte[32];

        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        
        randomNumberGenerator.GetBytes(randomNumberBytes);
        
        return Convert.ToBase64String(randomNumberBytes);
    }
}