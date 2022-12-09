using BookCatalog.Services.Models;

namespace BookCatalog.Services.Validators;

public class BookAuthorDtoValidator : DtoValidatorBase<BookAuthorDto>
{
    protected override bool ValidateIdCore(BookAuthorDto dto) => true;

    protected override bool ValidateCore(BookAuthorDto dto, out string error)
    {
        if (string.IsNullOrEmpty(dto.FirstName))
        {
            error = ValueMustBeProvidedError(nameof(dto.FirstName));
            return false;
        }
        
        if (string.IsNullOrEmpty(dto.LastName))
        {
            error = ValueMustBeProvidedError(nameof(dto.LastName));
            return false;
        }

        error = default;
        return true;
    }
}