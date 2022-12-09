using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class UpdateUserApiParameters
{
    [Required]
    public string Password { get; set; }
    [Required]
    public string NewName { get; set; }
    [Required]
    public string NewPassword { get; set; }
}