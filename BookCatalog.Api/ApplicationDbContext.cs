using BookCatalog.Storage;
using Microsoft.AspNetCore.Identity;

namespace BookCatalog.Api;

public class ApplicationDbContext : DataDbContext
{
    public ApplicationDbContext(IConfiguration configuration, IPasswordHasher<object> passwordHasher) 
        : base(configuration, passwordHasher)
    {
    }
}