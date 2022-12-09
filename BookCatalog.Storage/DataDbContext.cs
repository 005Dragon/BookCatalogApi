using BookCatalog.Storage.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookCatalog.Storage;

public abstract class DataDbContext : DbContext
{
    public DbSet<UserRecord> Users { get; set; }
    public DbSet<UserRoleRecord> UserRoles { get; set; }
    public DbSet<RoleRecord> Roles { get; set; }
    public DbSet<UserRefreshTokenRecord> UserRefreshTokens { get; set; }
    public DbSet<BookRecord> Books { get; set; }
    public DbSet<BookAuthorRecord> BookAuthors { get; set; }
    public DbSet<AuthorRecord> Authors { get; set; }
    public DbSet<BookCoverRecord> BookCovers { get; set; }
    public DbSet<CoverRecord> Covers { get; set; }


    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<object> _passwordHasher;

    protected DataDbContext(IConfiguration configuration, IPasswordHasher<object> passwordHasher)
    {
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRecord>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<UserRoleRecord>().HasIndex(x => x.UserId).IsUnique();
        modelBuilder.Entity<UserRefreshTokenRecord>().HasIndex(x => x.UserId).IsUnique();
        modelBuilder.Entity<BookRecord>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<AuthorRecord>().HasIndex(x => new { x.FirstName, x.LastName }).IsUnique();
        modelBuilder.Entity<BookCoverRecord>().HasIndex(x => x.BookId).IsUnique();
        
        CreatePresetData(modelBuilder);
    }

    private void CreatePresetData(ModelBuilder modelBuilder)
    {
        var adminUser = new UserRecord { Id = 1, Name = "admin" };
        adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, adminUser.Name);
        modelBuilder.Entity<UserRecord>().HasData(adminUser);

        var adminRole = new RoleRecord { Id = 1, Name = "admin" };
        var userRole = new RoleRecord { Id = 2, Name = "user" };
        modelBuilder.Entity<RoleRecord>().HasData(adminRole, userRole);

        modelBuilder.Entity<UserRoleRecord>()
            .HasData(new UserRoleRecord { Id = 1, UserId = adminUser.Id, RoleId = adminRole.Id});
    }
}