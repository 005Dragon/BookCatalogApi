using BookCatalog.Services.Models;

namespace BookCatalog.Services.Validators;

public class UserRefreshTokenDtoValidator : DtoValidatorBase<UserRefreshTokenDto>
{
    protected override bool ValidateCore(UserRefreshTokenDto dto, out string error)
    {
        if (string.IsNullOrEmpty(dto.Token))
        {
            error = ValueMustBeProvidedError(nameof(dto.Token));
            return false;
        }

        if (dto.ExpirationDateTime == default)
        {
            error = ValueMustBeProvidedError(nameof(dto.ExpirationDateTime));
            return false;
        }

        error = default;
        return true;
    }
}