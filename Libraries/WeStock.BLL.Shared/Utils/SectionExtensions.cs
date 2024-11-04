using WeStock.BLL.Shared.Models;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.Shared.Utils;

public static class SectionExtensions
{
    public static Section MapToModel(
        this ISectionEntity entity,
        int? collectionId = null
    ) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        CollectionId = collectionId ?? entity.CollectionId
    };

    public static TEntity MapToEntity<TEntity>(
        this Section model
    ) where TEntity : ISectionEntity, new()
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            CollectionId = model.CollectionId
        };

    public static TEntity MapToCreateEntity<TEntity>(
        this Section model,
        int? collectionId = null
    ) where TEntity : ISectionEntity, new()
        => new()
        {
            Name = model.Name,
            CollectionId = collectionId ?? model.CollectionId
        };
}