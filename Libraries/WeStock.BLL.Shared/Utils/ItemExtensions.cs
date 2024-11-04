using WeStock.BLL.Shared.Models;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.Shared.Utils;

public static class ItemExtensions
{
    public static Item MapToModel(
        this IItemEntity entity,
        int? sectionId = null
    ) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Quantity = entity.Quantity,
        SectionId = sectionId ?? entity.SectionId
    };

    public static TEntity MapToEntity<TEntity>(
        this Item model
    ) where TEntity : IItemEntity, new()
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Quantity = model.Quantity,
            SectionId = model.SectionId
        };

    public static TEntity MapToCreateEntity<TEntity>(
        this Item model,
        int? sectionId = null
    ) where TEntity : IItemEntity, new()
        => new()
        {
            Name = model.Name,
            Quantity = model.Quantity,
            SectionId = sectionId ?? model.SectionId
        };
}