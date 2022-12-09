using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class LoginApiParametersValidator : ApiParametersValidatorBase<LoginApiParameters>
{
    protected override bool ValidateCore(LoginApiParameters parameters, out IActionResult errorActionResult)
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

        errorActionResult = default;
        return true;
    }
}