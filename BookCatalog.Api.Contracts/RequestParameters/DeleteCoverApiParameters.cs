using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class DeleteCoverApiParameters
{
    [Required]
    public int CoverId { get; set; }
}