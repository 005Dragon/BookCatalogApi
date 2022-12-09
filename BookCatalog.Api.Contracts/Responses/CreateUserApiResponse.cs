namespace BookCatalog.Api.Contracts.Responses;

public class CreateUserApiResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}