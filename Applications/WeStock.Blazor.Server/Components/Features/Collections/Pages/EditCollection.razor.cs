using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Pages;

public partial class EditCollection
{
    [SupplyParameterFromQuery(Name = "id")]
    private int CollectionId { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ICollectionService CollectionService { get; set; } = null!;

    private CollectionViewModel? _collection;

    // TODO: CollectionDetails and EditCollection 'OnInitializedAsync' are identical. Can it be reused?

    protected override async Task OnInitializedAsync()
    {
        var collectionDto = await CollectionService.RetrieveCollectionByIdAsync(CollectionId);
        if (collectionDto is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }

        _collection = collectionDto.MapToViewModel();
    }

    private async Task UpdateCollection()
    {
        if (_collection is null)
            return;

        var updated = await CollectionService.UpdateCollectionAsync(_collection.MapToDto());
        if (updated)
            await NavigateBack();

        // TODO: else show error alert
    }

    private async Task NavigateBack()
    {
        await JsRuntime.InvokeVoidAsync("goBack");
    }
}