using BookCatalog.Services.Models;

namespace BookCatalog.Api.Contracts.Responses;

public class GetBooksApiResponse
{
    public List<BookDto> Books { get; set; }
}