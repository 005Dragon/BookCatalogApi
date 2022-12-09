using BookCatalog.Services.Models;

namespace BookCatalog.Services.Contracts;

public interface ICoverService
{
    CoverDto Create(string name, string imageType, byte[] imageData);
    bool Exists(int coverId);
    bool TryGetCover(int coverId, out CoverDto cover);
    void Save(CoverDto cover);
    void Delete(int coverId);
}