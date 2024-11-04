namespace WeStock.BLL.Shared.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int SectionId { get; set; }
}