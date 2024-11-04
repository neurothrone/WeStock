using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Entities;

public class InMemoryItemEntity : IItemEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int SectionId { get; set; }
}