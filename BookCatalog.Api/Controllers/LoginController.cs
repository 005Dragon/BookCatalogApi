using System.Security.Claims;
using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Api.Contracts.Validators;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Controllers;

[ApiController]
[Route(template: "[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserService _userService;

    public LoginController(
        IAccessTokenService accessTokenService, 
        IRefreshTokenService refreshTokenService, 
        IUserService userService)
    {
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginApiParameters parameters)
    {
        if (!new LoginApiParametersValidator().Validate(parameters, out IActionResult actionResult))
        {
            return actionResult;
        }

        if (!_userService.TryGetModel(parameters.Name, parameters.Password, out UserDto user))
        {
            return Unauthorized(new ApiErrorResponse { ErrorMessage = "Invalid login or password." });
        }

        DateTime utcNow = DateTime.UtcNow;

        user.RefreshToken = _refreshTokenService.Create(utcNow);
        
        _userService.Save(user);

        var claims = new List<Claim>
        {
            new(type: ClaimTypes.Name, value: user.Name),
            new(type: ClaimTypes.Role, value: user.Role)
        };

        var loginApiResponse = new LoginApiResponse
        {
            AccessToken = _accessTokenService.Create(claims, utcNow),
            RefreshToken = user.RefreshToken.Token
        };
            
        return Ok(loginApiResponse);
    }
}