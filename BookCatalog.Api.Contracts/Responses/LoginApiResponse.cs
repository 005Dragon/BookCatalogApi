namespace BookCatalog.Api.Contracts.Responses;

public class LoginApiResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}