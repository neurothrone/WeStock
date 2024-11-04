using System.ComponentModel.DataAnnotations;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Entities;

public class ItemEntity : IItemEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public int SectionId { get; set; }
    public List<SectionEntity> Sections { get; set; } = [];
}