namespace BookCatalog.Services.Models;

public class CoverDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageType { get; set; }
    public byte[] ImageData { get; set; }
}