using BookCatalog.Services.Models;

namespace BookCatalog.Services.Validators;

public class CoverDtoValidator : DtoValidatorBase<CoverDto>
{
    protected override bool ValidateIdCore(CoverDto dto) => dto.Id != default;

    protected override bool ValidateCore(CoverDto dto, out string error)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            error = ValueMustBeProvidedError(nameof(dto.Name));
            return false;
        }
        
        if (string.IsNullOrEmpty(dto.ImageType))
        {
            error = ValueMustBeProvidedError(nameof(dto.ImageType));
            return false;
        }
        
        if (dto.ImageData == null || dto.ImageData.Length == 0)
        {
            error = ValueMustBeProvidedError(nameof(dto.ImageType));
            return false;
        }

        error = default;
        return true;
    }
}