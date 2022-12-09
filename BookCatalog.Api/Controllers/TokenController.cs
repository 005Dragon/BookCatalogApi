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
public class TokenController : ControllerBase
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserService _userService;

    public TokenController(
        IAccessTokenService accessTokenService, 
        IRefreshTokenService refreshTokenService, 
        IUserService userService)
    {
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _userService = userService;
    }

    [HttpPost(template: nameof(Refresh))]
    public IActionResult Refresh([FromBody] RefreshTokenApiParameters parameters)
    {
        if (!new RefreshTokenApiParametersValidator().Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }

        ClaimsPrincipal principal;

        try
        {
            principal = _accessTokenService.GetPrincipal(parameters.AccessToken);
        }
        catch (Exception exception)
        {
            return Problem(detail: exception.Message);
        }

        if (principal.Identity == null)
        {
            return BadRequest();
        }

        string userName = principal.Identity.Name;

        if (!_userService.TryGetModel(userName, out UserDto user))
        {
            return Unauthorized();
        }

        if (!user.RefreshToken.Token.Equals(parameters.RefreshToken, StringComparison.Ordinal))
        {
            return Unauthorized();
        }

        DateTime utcNow = DateTime.UtcNow;

        if (user.RefreshToken.ExpirationDateTime < utcNow)
        {
            return Unauthorized();
        }

        user.RefreshToken = _refreshTokenService.Create(utcNow);
        _userService.Save(user);

        var refreshTokenApiResponse = new RefreshTokenApiResponse()
        {
            AccessToken = _accessTokenService.Create(principal.Claims, utcNow),
            RefreshToken = user.RefreshToken.Token
        };

        return Ok(refreshTokenApiResponse);
    }
}