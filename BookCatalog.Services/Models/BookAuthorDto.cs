using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Services.Models;

public class BookAuthorDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}