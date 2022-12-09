using Microsoft.Extensions.Configuration;

namespace BookCatalog.Services.Contracts;

public interface IPresetDataConfiguration
{
    string DefaultUserRole { get; }
}