using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class UserRefreshTokenRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(UserRecord))]
    public int UserId { get; set; }
    
    public UserRecord User { get; set; }
    
    [Required]
    public string Token { get; set; }

    public DateTime ExpirationDateTime { get; set; }
}