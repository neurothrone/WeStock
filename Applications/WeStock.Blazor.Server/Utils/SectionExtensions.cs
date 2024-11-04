using WeStock.Blazor.Server.ViewModels.Sections;
using WeStock.DTO.Section;

namespace WeStock.Blazor.Server.Utils;

public static class SectionExtensions
{
    public static SectionViewModel MapToViewModel(
        this SectionDto dto
    ) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        CollectionId = dto.CollectionId
    };

    public static CreateSectionDto MapToCreateDto(
        this SectionViewModel viewModel,
        int? collectionId = null
    ) => new(
        Name: viewModel.Name,
        CollectionId: collectionId ?? viewModel.CollectionId
    );

    public static SectionDto MapToDto(
        this SectionViewModel viewModel,
        int? collectionId = null
    ) => new(
        Id: viewModel.Id,
        Name: viewModel.Name,
        CollectionId: collectionId ?? viewModel.CollectionId
    );
}