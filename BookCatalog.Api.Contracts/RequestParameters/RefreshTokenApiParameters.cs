using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class RefreshTokenApiParameters
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}