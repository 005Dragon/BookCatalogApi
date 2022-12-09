using BookCatalog.Services.Models;

namespace BookCatalog.Services.Validators;

public class UserDtoValidator : DtoValidatorBase<UserDto>
{
    private readonly UserRefreshTokenDtoValidator _userRefreshTokenDtoValidator = new();

    protected override bool ValidateIdCore(UserDto dto) => dto.Id != default;

    protected override bool ValidateCore(UserDto dto, out string error)
    {
        if (string.IsNullOrEmpty(dto.Name))
        {
            error = ValueMustBeProvidedError(nameof(dto.Name));
            return false;
        }
        
        if (string.IsNullOrEmpty(dto.PasswordHash))
        {
            error = ValueMustBeProvidedError(nameof(dto.PasswordHash));
            return false;
        }

        if (dto.RefreshToken != null)
        {
            if (!_userRefreshTokenDtoValidator.Validate(dto.RefreshToken, out string refreshTokenError))
            {
                error = $"Invalid {dto.RefreshToken}. {refreshTokenError}";
                return false;
            }
        }
        
        error = default;
        return true;
    }
}