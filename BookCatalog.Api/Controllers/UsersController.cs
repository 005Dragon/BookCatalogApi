using System.Security.Claims;
using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Api.Contracts.Validators;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Controllers;

[ApiController]
[Route(template: "[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAccessTokenService _accessTokenService;
    private readonly IUserService _userService;

    public UsersController(IAccessTokenService accessTokenService, IUserService userService)
    {
        _accessTokenService = accessTokenService;
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserApiParameters parameters)
    {
        var registrationApiParametersValidator = new CreateUserApiParametersValidator(_userService);

        if (!registrationApiParametersValidator.Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }

        DateTime utcNow = DateTime.UtcNow;

        UserDto user = _userService.Create(parameters.Name, parameters.Password, utcNow);
        _userService.Save(user);

        var claims = new List<Claim>
        {
            new(type: ClaimTypes.Name, value: user.Name),
            new(type: ClaimTypes.Role, value: user.Role ?? string.Empty)
        };

        var response = new CreateUserApiResponse
        {
            AccessToken = _accessTokenService.Create(claims, utcNow),
            RefreshToken = user.RefreshToken.Token
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public IActionResult Update([FromBody] UpdateUserApiParameters parameters)
    {
        if (!new UpdateUserApiParametersValidator().Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }

        if (User.Identity == null)
        {
            return Unauthorized();
        }

        if (!_userService.TryGetModel(User.Identity.Name, parameters.Password, out UserDto user))
        {
            return BadRequest(new ApiErrorResponse { ErrorMessage = "Invalid password." });
        }

        user.Name = parameters.NewName;
        user.PasswordHash = _userService.HashPassword(parameters.NewPassword);

        _userService.Save(user);

        return Ok();
    }
}