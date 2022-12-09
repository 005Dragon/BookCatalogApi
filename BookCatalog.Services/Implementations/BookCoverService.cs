using BookCatalog.Services.Contracts;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class BookCoverService : IBookCoverService
{
    private readonly DataDbContext _dataDbContext;

    public BookCoverService(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public BookCoverRecord CreateRecord(int coverId)
    {
        return new BookCoverRecord { CoverId = coverId };
    }

    public void UpdateRecord(BookCoverRecord record, int coverId)
    {
        record.CoverId = coverId;
    }

    public void DeleteRecord(BookCoverRecord record)
    {
        _dataDbContext.BookCovers.Remove(record);
    }
}