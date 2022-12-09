using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Validators;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class UpdateBookApiParametersValidator : ApiParametersValidatorBase<UpdateBookApiParameters>
{
    private readonly BookDtoValidator _bookDtoValidator;

    public UpdateBookApiParametersValidator(ICoverService coverService)
    {
        _bookDtoValidator = new BookDtoValidator(coverService)
        {
            ValidateId = true
        };
    }

    protected override bool ValidateCore(UpdateBookApiParameters parameters, out IActionResult errorActionResult)
    {
        if (parameters.Book == null)
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.Book)));

            return false;
        }

        if (!_bookDtoValidator.Validate(parameters.Book, out string error))
        {
            errorActionResult = new BadRequestObjectResult(
                new ApiErrorResponse
                {
                    ErrorMessage = $"Invalid {nameof(parameters.Book)}. {error}"
                }
            );
            
            return false;
        }

        errorActionResult = default;
        return true;
    }
}