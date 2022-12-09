namespace BookCatalog.Services.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    
    public UserRefreshTokenDto RefreshToken { get; set; }
}