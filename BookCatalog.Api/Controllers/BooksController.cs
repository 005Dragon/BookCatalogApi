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
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ICoverService _coverService;

    public BooksController(IBookService bookService, ICoverService coverService)
    {
        _bookService = bookService;
        _coverService = coverService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult CreateBook([FromBody]CreateBookApiParameters parameters)
    {
        var createBookApiParametersValidator = new CreateBookApiParametersValidator(_coverService);

        if (!createBookApiParametersValidator.Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }

        _bookService.Save(parameters.Book);

        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpPut]
    public IActionResult UpdateBook([FromBody]UpdateBookApiParameters parameters)
    {
        var updateBookApiParametersValidator = new UpdateBookApiParametersValidator(_coverService);

        if (!updateBookApiParametersValidator.Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }
        
        _bookService.Save(parameters.Book);

        return Ok();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete]
    public IActionResult DeleteBooks([FromBody]DeleteBookApiParameters parameters)
    {
        var deleteBookApiParametersValidator = new DeleteBookApiParametersValidator(_bookService);

        if (!deleteBookApiParametersValidator.Validate(parameters, out IActionResult errorActionResult))
        {
            return errorActionResult;
        }
        
        _bookService.Delete(parameters.BookIds);
        
        return Ok();
    }
    
    [HttpGet]
    public IActionResult GetBooks()
    {
        List<BookDto> books = _bookService.GetBooks();

        var response = new GetBooksApiResponse { Books = books };

        return Ok(response);
    }
}