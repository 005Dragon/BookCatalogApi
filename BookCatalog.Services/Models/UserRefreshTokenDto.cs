namespace BookCatalog.Services.Models;

public class UserRefreshTokenDto
{
    public string Token { get; set; }

    public DateTime ExpirationDateTime { get; set; }
}