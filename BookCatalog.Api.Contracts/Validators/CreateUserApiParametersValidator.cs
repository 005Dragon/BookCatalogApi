using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class CreateUserApiParametersValidator : ApiParametersValidatorBase<CreateUserApiParameters>
{
    private readonly IUserService _userService;

    public CreateUserApiParametersValidator(IUserService userService)
    {
        _userService = userService;
    }

    protected override bool ValidateCore(CreateUserApiParameters parameters, out IActionResult errorActionResult)
    {
        if (string.IsNullOrEmpty(parameters.Name))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.Name)));
            
            return false;
        }

        if (string.IsNullOrEmpty(parameters.Password))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.Password)));
            
            return false;
        }
        
        if (_userService.Exists(parameters.Name))
        {
            errorActionResult = new BadRequestObjectResult(
                new ApiErrorResponse
                {
                    ErrorMessage = $"User with {nameof(parameters.Name)} - {parameters.Name} already exists."
                }
            );

            return false;
        }

        errorActionResult = default;
        return true;
    }
}