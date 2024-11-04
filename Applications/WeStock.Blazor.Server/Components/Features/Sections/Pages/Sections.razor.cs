using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Sections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Sections.Pages;

public partial class Sections
{
    [Inject]
    private ISectionService SectionService { get; set; } = null!;

    private List<SectionViewModel>? _sections;

    protected override async Task OnInitializedAsync()
    {
        var sections = await SectionService.RetrieveSectionsAsync();
        _sections = sections
            .Select(sectionDto => sectionDto.MapToViewModel())
            .ToList();
    }

    private async Task DeleteSection(SectionViewModel section)
    {
        var deleted = await SectionService.DeleteSectionByIdAsync(section.Id);
        if (!deleted)
            return;

        _sections?.Remove(section);
    }
}