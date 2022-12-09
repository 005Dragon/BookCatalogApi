using BookCatalog.Api.Contracts.RequestParameters;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class UpdateUserApiParametersValidator : ApiParametersValidatorBase<UpdateUserApiParameters>
{
    protected override bool ValidateCore(UpdateUserApiParameters parameters, out IActionResult errorActionResult)
    {
        if (string.IsNullOrEmpty(parameters.NewName))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.NewName)));
            
            return false;
        }
        
        if (string.IsNullOrEmpty(parameters.NewPassword))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.NewPassword)));
            
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