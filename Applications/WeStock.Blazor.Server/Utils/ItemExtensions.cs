using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.DTO.Item;

namespace WeStock.Blazor.Server.Utils;

public static class ItemExtensions
{
    public static ItemViewModel MapToViewModel(
        this ItemDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Quantity = dto.Quantity,
        SectionId = dto.SectionId
    };

    public static CreateItemDto MapToCreateDto(
        this ItemViewModel viewModel,
        int? sectionId = null
    ) => new(
        Name: viewModel.Name,
        Quantity: viewModel.Quantity,
        SectionId: sectionId ?? viewModel.SectionId
    );

    public static ItemDto MapToDto(
        this ItemViewModel viewModel,
        int? sectionId = null
    ) => new(
        Id: viewModel.Id,
        Name: viewModel.Name,
        Quantity: viewModel.Quantity,
        SectionId: sectionId ?? viewModel.SectionId
    );
}