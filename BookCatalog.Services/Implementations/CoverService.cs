using System.ComponentModel.DataAnnotations;
using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;
using BookCatalog.Services.Validators;
using BookCatalog.Storage;
using BookCatalog.Storage.Records;

namespace BookCatalog.Services.Implementations;

public class CoverService : ICoverService
{
    private readonly DataDbContext _dataDbContext;

    public CoverService(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public CoverDto Create(string name, string imageType, byte[] imageData)
    {
        return new CoverDto
        {
            Name = name,
            ImageType = imageType,
            ImageData = imageData
        };
    }

    public bool Exists(int coverId) => _dataDbContext.Covers.Any(x => x.Id == coverId);

    public bool TryGetCover(int coverId, out CoverDto cover)
    {
        CoverRecord coverRecord = _dataDbContext.Covers.Find(coverId);

        if (coverRecord == null)
        {
            cover = default;
            return false;
        }

        cover = Create(coverRecord);
        return true;
    }

    public void Save(CoverDto cover)
    {
        if (!new CoverDtoValidator().Validate(cover, out string error))
        {
            throw new ValidationException(error);
        }
        
        CoverRecord coverRecord = cover.Id == default ? CreateRecord(cover) : UpdateRecord(cover);
        
        _dataDbContext.SaveChanges();
        cover.Id = coverRecord.Id;
    }

    private CoverRecord UpdateRecord(CoverDto cover)
    {
        CoverRecord coverRecord = _dataDbContext.Covers.Find(cover.Id);

        if (coverRecord == null)
        {
            throw new KeyNotFoundException($"cover with id: {cover.Id} not found.");
        }

        coverRecord.Name = cover.Name;
        coverRecord.ImageType = cover.ImageType;
        coverRecord.ImageData = cover.ImageData;

        return coverRecord;
    }

    public void Delete(int coverId)
    {
        CoverRecord coverRecord = _dataDbContext.Covers.Find(coverId);

        if (coverRecord == null)
        {
            throw new KeyNotFoundException();
        }

        _dataDbContext.Covers.Remove(coverRecord);
        _dataDbContext.SaveChanges();
    }

    private CoverRecord CreateRecord(CoverDto cover)
    {
        var coverRecord = new CoverRecord
        {
            Name = cover.Name,
            ImageType = cover.ImageType,
            ImageData = cover.ImageData
        };

        _dataDbContext.Covers.Add(coverRecord);

        return coverRecord;
    }

    private static CoverDto Create(CoverRecord record)
    {
        return new CoverDto
        {
            Id = record.Id,
            Name = record.Name,
            ImageType = record.ImageType,
            ImageData = record.ImageData
        };
    }
}