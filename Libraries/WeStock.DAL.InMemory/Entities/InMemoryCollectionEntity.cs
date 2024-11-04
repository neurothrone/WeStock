using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Entities;

public class InMemoryCollectionEntity : ICollectionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}