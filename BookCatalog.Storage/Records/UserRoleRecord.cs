using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.Storage.Records;

public class UserRoleRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(UserRecord))]
    public int UserId { get; set; }
    public UserRecord User { get; set; }
    
    [ForeignKey(nameof(RoleRecord))]
    public int RoleId { get; set; }
    public RoleRecord Role { get; set; }
}