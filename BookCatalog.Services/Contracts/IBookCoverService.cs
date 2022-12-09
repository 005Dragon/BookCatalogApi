using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Contracts;

public interface IBookCoverService
{
    internal BookCoverRecord CreateRecord(int coverId);
    internal void UpdateRecord(BookCoverRecord record, int coverId);
    internal void DeleteRecord(BookCoverRecord record);
}