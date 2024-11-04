using WeStock.BLL.Shared.Models;
using WeStock.DTO.Collection;

namespace WeStock.SL.Utils;

public static class CollectionExtensions
{
    public static Collection MapToModel(
        this CollectionDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name
    };

    public static Collection MapToCreateModel(
        this CreateCollectionDto dto
    ) => new()
    {
        Name = dto.Name
    };

    public static CollectionDto MapToDto(
        this Collection model
    ) => new(
        model.Id,
        model.Name
    );
}