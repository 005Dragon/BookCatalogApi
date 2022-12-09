using BookCatalog.Services.Models;

namespace BookCatalog.Services.Contracts;

public interface IUserService
{
    UserDto Create(string name, string password, DateTime utcNow);
    bool Exists(string name);
    bool TryGetModel(string name, out UserDto user);
    bool TryGetModel(string name, string password, out UserDto user);
    void Save(UserDto userDto);
    string HashPassword(string password);
}