using BookCatalog.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public abstract class ApiParametersValidatorBase<TParameters>
{
    public bool Validate(TParameters parameters, out IActionResult errorActionResult)
    {
        if (parameters == null)
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters)));
            
            return false;
        }

        return ValidateCore(parameters, out errorActionResult);
    }

    protected abstract bool ValidateCore(TParameters parameters, out IActionResult errorActionResult);

    protected ApiErrorResponse ValueMustBeProvidedError(string propertyName)
    {
        return new ApiErrorResponse { ErrorMessage = $"Value {propertyName} must be provided." };
    }
}