using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class CreateCoverApiParametersValidator : ApiParametersValidatorBase<IFormFile>
{
    protected override bool ValidateCore(IFormFile parameters, out IActionResult errorActionResult)
    {
        if (string.IsNullOrEmpty(parameters.FileName))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.FileName)));

            return false;
        }

        if (string.IsNullOrEmpty(parameters.ContentType))
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.ContentType)));

            return false;
        }

        errorActionResult = default;
        return true;
    }
}