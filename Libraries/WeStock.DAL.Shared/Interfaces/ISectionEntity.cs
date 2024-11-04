namespace WeStock.DAL.Shared.Interfaces;

public interface ISectionEntity
{
    int Id { get; set; }
    string Name { get; set; }
    int CollectionId { get; set; }
}