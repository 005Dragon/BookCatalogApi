using BookCatalog.Services.Models;

namespace BookCatalog.Services.Contracts;

public interface IBookService
{
    bool Exists(ICollection<int> ids, out List<int> notExistIds);

    List<BookDto> GetBooks();

    void Save(BookDto book);

    void Delete(ICollection<int> bookIds);
}