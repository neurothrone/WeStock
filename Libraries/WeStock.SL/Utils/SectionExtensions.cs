using WeStock.BLL.Shared.Models;
using WeStock.DTO.Section;

namespace WeStock.SL.Utils;

public static class SectionExtensions
{
    public static Section MapToModel(
        this SectionDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        CollectionId = dto.CollectionId
    };

    public static Section MapToCreateModel(
        this CreateSectionDto dto,
        int? collectionId = null
    ) => new()
    {
        Name = dto.Name,
        CollectionId = collectionId ?? dto.CollectionId
    };

    public static SectionDto MapToDto(
        this Section model,
        int? collectionId = null
    ) => new(
        Id: model.Id,
        Name: model.Name,
        CollectionId: collectionId ?? model.CollectionId
    );
}