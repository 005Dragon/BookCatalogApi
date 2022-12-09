using BookCatalog.Api.Configurations;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Implementations;
using BookCatalog.Storage;
using Microsoft.AspNetCore.Identity;

namespace BookCatalog.Api;

public static class TransientInitializer
{
    public static void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IAuthenticationConfiguration, AuthenticationConfiguration>();
        builder.Services.AddTransient<IPresetDataConfiguration, PresetDataConfiguration>();
        builder.Services.AddTransient<IPasswordHasher<object>, PasswordHasher<object>>();
        
        builder.Services.AddTransient<DataDbContext, ApplicationDbContext>();
        
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAccessTokenService, AccessTokenService>();
        builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();
        builder.Services.AddTransient<IUserRoleService, UserRoleService>();
        builder.Services.AddTransient<IBookService, BookService>();
        builder.Services.AddTransient<IBookAuthorService, BookAuthorService>();
        builder.Services.AddTransient<IAuthorService, AuthorService>();
        builder.Services.AddTransient<IBookCoverService, BookCoverService>();
        builder.Services.AddTransient<ICoverService, CoverService>();
    }
}