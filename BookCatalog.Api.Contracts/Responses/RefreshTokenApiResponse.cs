namespace BookCatalog.Api.Contracts.Responses;

public class RefreshTokenApiResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}