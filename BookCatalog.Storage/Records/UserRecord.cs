using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class UserRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public UserRoleRecord UserRole { get; set; }
    public UserRefreshTokenRecord UserRefreshToken { get; set; }
}