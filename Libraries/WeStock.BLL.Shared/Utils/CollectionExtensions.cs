using WeStock.BLL.Shared.Models;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.Shared.Utils;

public static class CollectionExtensions
{
    public static Collection MapToModel(
        this ICollectionEntity entity
    ) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static TEntity MapToEntity<TEntity>(
        this Collection model
    ) where TEntity : ICollectionEntity, new()
        => new()
        {
            Id = model.Id,
            Name = model.Name
        };

    public static TEntity MapToCreateEntity<TEntity>(
        this Collection model
    ) where TEntity : ICollectionEntity, new()
        => new()
        {
            Name = model.Name
        };
}