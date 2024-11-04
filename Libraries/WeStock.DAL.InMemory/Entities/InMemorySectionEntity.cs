using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Entities;

public class InMemorySectionEntity : ISectionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CollectionId { get; set; }
}