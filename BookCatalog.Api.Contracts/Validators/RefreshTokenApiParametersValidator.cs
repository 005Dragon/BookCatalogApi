using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class RefreshTokenApiParametersValidator : ApiParametersValidatorBase<RefreshTokenApiParameters>
{
    protected override bool ValidateCore(RefreshTokenApiParameters parameters, out IActionResult errorActionResult)
    {
        if (string.IsNullOrEmpty(parameters.AccessToken))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.AccessToken)));
            
            return false;
        }
        
        if (string.IsNullOrEmpty(parameters.AccessToken))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.AccessToken)));
            
            return false;
        }

        errorActionResult = default;
        return true;
    }
}