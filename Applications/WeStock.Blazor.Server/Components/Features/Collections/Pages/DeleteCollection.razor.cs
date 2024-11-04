using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Pages;

public partial class DeleteCollection
{
    [SupplyParameterFromQuery(Name = "id")]
    private int CollectionId { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ICollectionService CollectionService { get; set; } = null!;

    [Inject]
    private ISectionService SectionService { get; set; } = null!;

    private int _sectionsCount;
    private bool _busy = false;

    protected override async Task OnInitializedAsync()
    {
        _sectionsCount = await SectionService.RetrieveSectionsCountByCollectionIdAsync(CollectionId);
    }

    private async Task Delete()
    {
        _busy = true;

        try
        {
            var sections = await SectionService.RetrieveSectionsByCollectionIdAsync(CollectionId);
            foreach (var section in sections)
            {
                await SectionService.DeleteSectionByIdAsync(section.Id);
            }

            var deleted = await CollectionService.DeleteCollectionByIdAsync(CollectionId);
            if (deleted)
                await NavigateBack();
        }
        finally
        {
            _busy = false;
        }
    }

    private async Task NavigateBack()
    {
        // TODO: NavigationService? GoBack method
        await JsRuntime.InvokeVoidAsync("goBack");
    }
}