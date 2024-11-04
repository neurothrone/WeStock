namespace WeStock.DAL.Shared.Interfaces;

public interface IItemEntity
{
    int Id { get; set; }
    string Name { get; set; }
    int Quantity { get; set; }
    int SectionId { get; set; }
}