using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.State;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Pages;

public partial class CollectionDetails : IDisposable
{
    [SupplyParameterFromQuery(Name = "id")]
    private int CollectionId { get; set; }

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private AppState AppState { get; set; } = null!;

    [Inject]
    private ICollectionService CollectionService { get; set; } = null!;

    private CollectionViewModel? _collection;
    private int _totalItemCount;

    protected override async Task OnInitializedAsync()
    {
        AppState.OnDataChanged += LoadItemCountInCollection;

        var collectionDto = await CollectionService.RetrieveCollectionByIdAsync(CollectionId);
        if (collectionDto is null)
        {
            Navigation.NavigateTo("/404");
            return;
        }

        _collection = collectionDto.MapToViewModel();
        await LoadItemCountInCollection();
    }

    private async Task LoadItemCountInCollection()
    {
        _totalItemCount = await CollectionService.GetItemCountInCollectionAsync(CollectionId);
        StateHasChanged();
    }

    #region IDisposable

    public void Dispose()
    {
        AppState.OnDataChanged -= LoadItemCountInCollection;
    }

    #endregion
}