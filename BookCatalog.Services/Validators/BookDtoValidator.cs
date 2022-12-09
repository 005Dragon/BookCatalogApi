using BookCatalog.Services.Contracts;
using BookCatalog.Services.Models;

namespace BookCatalog.Services.Validators;

public class BookDtoValidator : DtoValidatorBase<BookDto>
{
    private readonly BookAuthorDtoValidator _bookAuthorDtoValidator = new();
    private readonly ICoverService _coverService;

    public BookDtoValidator(ICoverService coverService)
    {
        _coverService = coverService;
    }

    protected override bool ValidateIdCore(BookDto dto) => dto.Id != default;

    protected override bool ValidateCore(BookDto dto, out string error)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            error = ValueMustBeProvidedError(nameof(dto.Name));
            return false;
        }

        if (dto.Year != null)
        {
            if (dto.Year <= 0)
            {
                error = $"Value {nameof(dto.Year)} must be more than 0.";
                return false;
            }
            
            if (dto.Year > DateTime.UtcNow.Year)
            {
                error = $"Value {nameof(dto.Year)} must be less than current year.";
                return false;
            }
        }

        if (dto.PageCount != null)
        {
            if (dto.PageCount <= 0)
            {
                error = $"Value {nameof(dto.PageCount)} must be more than 0.";
                return false;
            }
        }

        if (dto.Authors != null)
        {
            foreach (BookAuthorDto bookAuthor in dto.Authors)
            {
                if (!_bookAuthorDtoValidator.Validate(bookAuthor, out string bookAuthorError))
                {
                    error = $"Invalid {dto.Authors}. {bookAuthorError}";
                    return false;
                }
            }
        }

        if (dto.CoverId != null)
        {
            if (!_coverService.Exists((int)dto.CoverId))
            {
                error = $"Invalid {nameof(dto.CoverId)} - {dto.CoverId}";
                return false;
            }
        }

        error = default;
        return true;
    }
}