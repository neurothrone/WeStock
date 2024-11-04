using System.ComponentModel.DataAnnotations;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.EFCore.Entities;

public class CollectionEntity : ICollectionEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public List<SectionEntity> Sections { get; set; } = [];
}