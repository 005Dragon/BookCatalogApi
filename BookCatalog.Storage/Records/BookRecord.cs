using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class BookRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int? Year { get; set; }
    
    public int? PageCount { get; set; }
    
    public string Description { get; set; }
    
    public int? CategoryFullness { get; set; }
    
    public ICollection<BookAuthorRecord> Authors { get; set; }
    
    public BookCoverRecord CoverRecord { get; set; }
}