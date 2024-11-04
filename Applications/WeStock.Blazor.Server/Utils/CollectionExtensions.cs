using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.DTO.Collection;

namespace WeStock.Blazor.Server.Utils;

public static class CollectionExtensions
{
    public static CollectionViewModel MapToViewModel(
        this CollectionDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name
    };

    public static CreateCollectionDto MapToCreateDto(
        this CollectionViewModel viewModel
    ) => new(
        viewModel.Name
    );

    public static CollectionDto MapToDto(
        this CollectionViewModel viewModel
    ) => new(
        viewModel.Id,
        viewModel.Name
    );
}