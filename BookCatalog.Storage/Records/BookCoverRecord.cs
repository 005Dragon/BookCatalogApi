using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class BookCoverRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(BookRecord))]
    public int BookId { get; set; }
    public BookRecord Book { get; set; }
    
    [ForeignKey(nameof(CoverRecord))]
    public int CoverId { get; set; }
    public CoverRecord Cover { get; set; }
}