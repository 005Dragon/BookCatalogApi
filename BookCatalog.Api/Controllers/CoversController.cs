using BookCatalog.Api.Contracts.RequestParameters;
using BookCatalog.Api.Contracts.Responses;
using BookCatalog.Api.Contracts.Validators;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Api.Controllers;

[Authorize]
[ApiController]
[Route(template: "[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoverService _coverService;

    public CoversController(ICoverService coverService)
    {
        _coverService = coverService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult CreateCover(IFormFile parameters)
    {
        if (!new CreateCoverApiParametersValidator().Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }

        byte[] imageData;
        
        using (Stream readStream = parameters.OpenReadStream())
        using (var memorySteam = new MemoryStream())
        {
            readStream.CopyTo(memorySteam);
            imageData = memorySteam.ToArray();
        }

        CoverDto cover = _coverService.Create(parameters.FileName, parameters.ContentType, imageData);
        _coverService.Save(cover);

        return Ok(new CreateCoverApiResponse { CoverId = cover.Id });
    }

    [Authorize(Roles = "admin")]
    [HttpDelete]
    public IActionResult DeleteCover([FromBody] DeleteCoverApiParameters parameters)
    {
        var deleteCoverApiParametersValidator = new DeleteCoverApiParametersValidator(_coverService);

        if (!deleteCoverApiParametersValidator.Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }
        
        _coverService.Delete(parameters.CoverId);

        return Ok();
    }

    [HttpGet]
    public IActionResult GetBookCover(int coverId)
    {
        if (coverId == default)
        {
            return BadRequest($"Value {nameof(coverId)} must be provided");
        }

        if (!_coverService.TryGetCover(coverId, out CoverDto cover))
        {
            return BadRequest($"Cover with id - {coverId} not exists.");
        }

        return File(cover.ImageData, cover.ImageType);
    }
}