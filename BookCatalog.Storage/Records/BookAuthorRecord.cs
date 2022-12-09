using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class BookAuthorRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(BookRecord))]
    public int BookId { get; set; }
    public BookRecord Book { get; set; }
    
    [ForeignKey(nameof(AuthorRecord))]
    public int AuthorId { get; set; }
    public AuthorRecord Author { get; set; } 
}