using WeStock.BLL.Shared.Models;
using WeStock.DTO.Item;

namespace WeStock.SL.Utils;

public static class ItemExtensions
{
    public static Item MapToModel(
        this ItemDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Quantity = dto.Quantity,
        SectionId = dto.SectionId
    };

    public static Item MapToCreateModel(
        this CreateItemDto dto,
        int? sectionId = null
    ) => new()
    {
        Name = dto.Name,
        Quantity = dto.Quantity,
        SectionId = sectionId ?? dto.SectionId
    };

    public static ItemDto MapToDto(
        this Item model,
        int? sectionId = null
    ) => new(
        Id: model.Id,
        Name: model.Name,
        Quantity: model.Quantity,
        SectionId: sectionId ?? model.SectionId
    );
}