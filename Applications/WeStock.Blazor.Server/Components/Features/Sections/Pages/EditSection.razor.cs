using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Sections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Sections.Pages;

public partial class EditSection
{
    [SupplyParameterFromQuery(Name = "id")]
    private int SectionId { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ISectionService SectionService { get; set; } = null!;

    private SectionViewModel? _section;

    protected override async Task OnInitializedAsync()
    {
        var sectionDto = await SectionService.RetrieveSectionByIdAsync(SectionId);
        if (sectionDto is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }

        _section = sectionDto.MapToViewModel();
    }

    private async Task UpdateSection()
    {
        if (_section is null)
            return;

        var updated = await SectionService.UpdateSectionAsync(_section.MapToDto());
        if (updated)
            await NavigateBack();

        // TODO: else show error alert
    }

    private async Task NavigateBack()
    {
        await JsRuntime.InvokeVoidAsync("goBack");
    }
}