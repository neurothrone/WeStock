namespace WeStock.BLL.Shared.Models;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CollectionId { get; set; }
}