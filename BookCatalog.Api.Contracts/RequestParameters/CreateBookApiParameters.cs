using System.ComponentModel.DataAnnotations;
using BookCatalog.Services.Models;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class CreateBookApiParameters
{
    [Required]
    public BookDto Book { get; set; }
}