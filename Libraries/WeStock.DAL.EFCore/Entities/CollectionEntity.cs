using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Entities;

public class CollectionEntity : ICollectionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<SectionEntity> Sections { get; set; } = [];
}