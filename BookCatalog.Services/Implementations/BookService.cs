using System.ComponentModel.DataAnnotations;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using BookCatalog.Services.Validators;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Services.Implementations;

public class BookService : IBookService
{
    private readonly DataDbContext _dataDbContext;
    private readonly IBookAuthorService _bookAuthorService;
    private readonly IBookCoverService _bookCoverService;
    private readonly ICoverService _coverService;

    public BookService(
        DataDbContext dataDbContext, 
        IBookAuthorService bookAuthorService, 
        IBookCoverService bookCoverService, 
        ICoverService coverService)
    {
        _dataDbContext = dataDbContext;
        _bookAuthorService = bookAuthorService;
        _bookCoverService = bookCoverService;
        _coverService = coverService;
    }

    public bool Exists(ICollection<int> ids, out List<int> notExistIds)
    {
        notExistIds = ids.Except(_dataDbContext.Books.Where(x => ids.Contains(x.Id)).Select(x => x.Id)).ToList();

        return notExistIds.Count == 0;
    }

    public List<BookDto> GetBooks()
    {
        return _dataDbContext.Books
            .Include(x => x.Authors).ThenInclude(x => x.Author)
            .Include(x => x.CoverRecord)
            .Select(CreateDto)
            .ToList();
    }

    public void Save(BookDto book)
    {
        var bookDtoValidator = new BookDtoValidator(_coverService);

        if (!bookDtoValidator.Validate(book, out string error))
        {
            throw new ValidationException(error);
        }

        BookRecord bookRecord = book.Id == default ? CreateRecord(book) : UpdateRecord(book);
        
        _dataDbContext.SaveChanges();
        book.Id = bookRecord.Id;
    }

    public void Delete(ICollection<int> bookIds)
    {
        List<BookRecord> bookRecords = _dataDbContext.Books.Where(x => bookIds.Contains(x.Id)).ToList();

        _dataDbContext.Books.RemoveRange(bookRecords);

        _dataDbContext.SaveChanges();
    }

    private BookRecord CreateRecord(BookDto book)
    {
        var bookRecord = new BookRecord
        {
            Name = book.Name,
            Year = book.Year,
            PageCount = book.PageCount,
            CategoryFullness = book.CategoryFullness,
            Description = book.Description,
            Authors = book.Authors?.Select(_bookAuthorService.CreateRecord).ToList(),
            CoverRecord = book.CoverId != null ? _bookCoverService.CreateRecord((int)book.CoverId) : null
        };

        _dataDbContext.Books.Add(bookRecord);

        return bookRecord;
    }

    private BookRecord UpdateRecord(BookDto book)
    {
        BookRecord bookRecord = _dataDbContext.Books
            .Where(x => x.Id == book.Id)
            .Include(x => x.Authors).ThenInclude(x => x.Author)
            .Include(x => x.CoverRecord)
            .FirstOrDefault();

        if (bookRecord == null)
        {
            throw new KeyNotFoundException($"User with id: {book.Id} not found.");
        }

        bookRecord.Name = book.Name;
        bookRecord.Year = book.Year;
        bookRecord.PageCount = book.PageCount;
        bookRecord.CategoryFullness = book.CategoryFullness;
        bookRecord.Description = book.Description;

        UpdateBookAuthor(bookRecord, book);
        UpdateBookCover(bookRecord, book);

        return bookRecord;
    }

    private void UpdateBookAuthor(BookRecord bookRecord, BookDto book)
    {
        List<BookAuthorDto> bookAuthors = book.Authors?.ToList() ?? new List<BookAuthorDto>();

        foreach (BookAuthorRecord bookAuthorRecord in bookRecord.Authors ?? Enumerable.Empty<BookAuthorRecord>())
        {
            BookAuthorDto bookAuthorDto =
                bookAuthors.FirstOrDefault(x => _bookAuthorService.Equals(bookAuthorRecord, x));

            if (bookAuthorDto == null)
            {
                _bookAuthorService.DeleteRecord(bookAuthorRecord);
            }
            else
            {
                _bookAuthorService.UpdateRecord(bookAuthorRecord, bookAuthorDto);
                bookAuthors.Remove(bookAuthorDto);
            }
        }

        foreach (BookAuthorDto bookAuthorDto in bookAuthors)
        { 
            _bookAuthorService.CreateRecord(bookAuthorDto);
        }
    }

    private void UpdateBookCover(BookRecord bookRecord, BookDto book)
    {
        if (bookRecord.CoverRecord == null)
        {
            if (book.CoverId != null)
            {
                bookRecord.CoverRecord = _bookCoverService.CreateRecord((int)book.CoverId);
            }
        }
        else
        {
            if (book.CoverId == null)
            {
                _bookCoverService.DeleteRecord(bookRecord.CoverRecord);
            }
            else
            {
                _bookCoverService.UpdateRecord(bookRecord.CoverRecord, (int)book.CoverId);
            }
        }
    }

    private BookDto CreateDto(BookRecord bookRecord)
    {
        return new BookDto
        {
            Id = bookRecord.Id,
            Name = bookRecord.Name,
            Year = bookRecord.Year,
            PageCount = bookRecord.PageCount,
            CategoryFullness = bookRecord.CategoryFullness,
            Description = bookRecord.Description,
            Authors = bookRecord.Authors?.Select(_bookAuthorService.Create).ToList(),
            CoverId = bookRecord.CoverRecord?.CoverId
        };
    }
}