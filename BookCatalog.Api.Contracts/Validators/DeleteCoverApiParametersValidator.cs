using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Contracts.Validators;

public class DeleteCoverApiParametersValidator : ApiParametersValidatorBase<DeleteCoverApiParameters>
{
    private readonly ICoverService _coverService;

    public DeleteCoverApiParametersValidator(ICoverService coverService)
    {
        _coverService = coverService;
    }

    protected override bool ValidateCore(DeleteCoverApiParameters parameters, out IActionResult errorActionResult)
    {
        if (parameters.CoverId == default)
        {
            errorActionResult = new BadRequestObjectResult(ValueMustBeProvidedError(nameof(parameters.CoverId)));
            return false;
        }

        if (!_coverService.Exists(parameters.CoverId))
        {
            errorActionResult = new BadRequestObjectResult(
                new ApiErrorResponse
                {
                    ErrorMessage = $"Cover with id - {parameters.CoverId} not exists."
                }
            );

            return false;
        }

        errorActionResult = default;
        return true;
    }
}