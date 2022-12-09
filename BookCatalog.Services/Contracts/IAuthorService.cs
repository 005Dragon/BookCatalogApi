using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Contracts;

public interface IAuthorService
{
    internal bool TryGetRecord(string firstName, string lastName, out AuthorRecord authorRecord);
    
    internal AuthorRecord Create(string firstName, string lastName);
}