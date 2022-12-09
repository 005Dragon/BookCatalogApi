using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Services.Models;

public class BookDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public int? Year { get; set; }
    
    public int? PageCount { get; set; }
    
    public string Description { get; set; }
    
    public int? CategoryFullness { get; set; }

    public int? CoverId { get; set; }
    public List<BookAuthorDto> Authors { get; set; }
}