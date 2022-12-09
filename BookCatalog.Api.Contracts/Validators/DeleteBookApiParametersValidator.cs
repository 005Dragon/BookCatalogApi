using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class DeleteBookApiParametersValidator : ApiParametersValidatorBase<DeleteBookApiParameters>
{
    private readonly IBookService _bookService;

    public DeleteBookApiParametersValidator(IBookService bookService)
    {
        _bookService = bookService;
    }

    protected override bool ValidateCore(DeleteBookApiParameters parameters, out IActionResult errorActionResult)
    {
        if (parameters.BookIds == null || parameters.BookIds.Count == 0)
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.BookIds)));
            
            return false;
        }

        if (!_bookService.Exists(parameters.BookIds, out List<int> notExistIds))
        {
            errorActionResult = new BadRequestObjectResult(
                new ApiErrorResponse
                {
                    ErrorMessage = $"Books with id ({string.Join(", ", notExistIds)}) not exists."
                }
            );
            
            return false;
        }

        errorActionResult = default;
        return true;
    }
}