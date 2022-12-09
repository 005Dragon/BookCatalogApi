using BookCatalog.Services.Contracts;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly DataDbContext _dataDbContext;

    public AuthorService(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public bool TryGetRecord(string firstName, string lastName, out AuthorRecord authorRecord)
    {
        authorRecord = _dataDbContext.Authors.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

        return authorRecord != null;
    }

    public AuthorRecord Create(string firstName, string lastName)
    {
        return new AuthorRecord
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
}