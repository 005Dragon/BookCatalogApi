namespace BookCatalog.Services.Validators;

public abstract class DtoValidatorBase<TDto>
{
    public bool ValidateId { get; init; }
    
    public bool Validate(TDto dto, out string error)
    {
        if (dto == null)
        {
            error = ValueMustBeProvidedError(nameof(dto));
            return false;
        }

        if (ValidateId && !ValidateIdCore(dto))
        {
            error = ValueMustBeProvidedError("Id");
            return false;
        }
        
        return ValidateCore(dto, out error);
    }

    protected virtual bool ValidateIdCore(TDto dto) => true;

    protected abstract bool ValidateCore(TDto dto, out string error);

    protected string ValueMustBeProvidedError(string propertyName)
    {
        return $"Value {propertyName} must be provided.";
    }
}