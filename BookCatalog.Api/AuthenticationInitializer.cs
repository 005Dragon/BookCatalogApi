using BookCatalog.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BookCatalog.Api;

public static class AuthenticationInitializer
{
    public static void Initialize(WebApplicationBuilder builder)
    {
        var authenticationConfiguration = new AuthenticationConfiguration(builder.Configuration);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authenticationConfiguration.TokenIssuer,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        LifetimeValidator = (_, expires, _, _) => expires > DateTime.UtcNow,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = authenticationConfiguration.GetSymmetricSecurityKey()
                    };
                }
            );
    }
}