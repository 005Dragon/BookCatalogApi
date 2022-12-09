using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class DeleteBookApiParameters
{
    [Required]
    public List<int> BookIds { get; set; }
}