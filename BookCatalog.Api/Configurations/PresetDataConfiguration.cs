using BookCatalog.Services.Contracts;

namespace BookCatalog.Api.Configurations;

public class PresetDataConfiguration : IPresetDataConfiguration
{
    public string DefaultUserRole => _configuration.GetSection(SectionName)[nameof(DefaultUserRole)];

    private const string SectionName = "PresetData";
    private readonly IConfiguration _configuration;

    public PresetDataConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}