using System.ComponentModel.DataAnnotations;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Entities;

public class SectionEntity : ISectionEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int CollectionId { get; set; }
    public CollectionEntity Collection { get; set; }
    public List<ItemEntity> Items { get; set; } = [];
}