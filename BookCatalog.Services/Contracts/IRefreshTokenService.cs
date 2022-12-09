using BookCatalog.Services.Models;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Contracts;

public interface IRefreshTokenService
{
    UserRefreshTokenDto Create(DateTime utcNow);
    internal UserRefreshTokenDto Create(UserRefreshTokenRecord userRefreshTokenRecord);
    internal UserRefreshTokenRecord CreateRecord(UserRefreshTokenDto refreshToken);
    internal void UpdateRecord(UserRefreshTokenRecord userRefreshTokenRecord, UserRefreshTokenDto userRefreshTokenDto);
}