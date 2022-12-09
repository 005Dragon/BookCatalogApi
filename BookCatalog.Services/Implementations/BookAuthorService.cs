using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class BookAuthorService : IBookAuthorService
{
    private readonly DataDbContext _dataDbContext;
    private readonly IAuthorService _authorService;

    public BookAuthorService(DataDbContext dataDbContext, IAuthorService authorService)
    {
        _dataDbContext = dataDbContext;
        _authorService = authorService;
    }

    public BookAuthorDto Create(BookAuthorRecord record)
    {
        return new BookAuthorDto
        {
            FirstName = record.Author.FirstName,
            LastName = record.Author.LastName
        };
    }

    public BookAuthorRecord CreateRecord(BookAuthorDto bookAuthor)
    {
        if (_authorService.TryGetRecord(bookAuthor.FirstName, bookAuthor.LastName, out AuthorRecord authorRecord))
        {
            return new BookAuthorRecord { AuthorId = authorRecord.Id };
        }

        authorRecord = _authorService.Create(bookAuthor.FirstName, bookAuthor.LastName);
        return new BookAuthorRecord { Author = authorRecord };
    }

    public void UpdateRecord(BookAuthorRecord bookAuthorRecord, BookAuthorDto bookAuthor)
    {
        if (_authorService.TryGetRecord(bookAuthor.FirstName, bookAuthor.LastName, out AuthorRecord authorRecord))
        {
            bookAuthorRecord.AuthorId = authorRecord.Id;
            return;
        }

        bookAuthorRecord.Author = _authorService.Create(bookAuthor.FirstName, bookAuthor.LastName);
    }

    public void DeleteRecord(BookAuthorRecord bookAuthorRecord)
    {
        _dataDbContext.BookAuthors.Remove(bookAuthorRecord);
    }

    public bool Equals(BookAuthorRecord bookAuthorRecord, BookAuthorDto bookAuthorDto)
    {
        AuthorRecord authorRecord = bookAuthorRecord.Author;

        return
            string.Equals(authorRecord.FirstName, bookAuthorDto.FirstName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(authorRecord.LastName, bookAuthorDto.LastName, StringComparison.OrdinalIgnoreCase);
    }
}