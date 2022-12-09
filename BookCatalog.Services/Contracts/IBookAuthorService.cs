using BookCatalog.Services.Models;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Contracts;

public interface IBookAuthorService
{
    internal BookAuthorDto Create(BookAuthorRecord record);
    internal BookAuthorRecord CreateRecord(BookAuthorDto bookAuthor);
    internal void UpdateRecord(BookAuthorRecord bookAuthorRecord, BookAuthorDto bookAuthorDto);
    internal void DeleteRecord(BookAuthorRecord bookAuthorRecord);
    internal bool Equals(BookAuthorRecord bookAuthorRecord, BookAuthorDto bookAuthorDto);
}