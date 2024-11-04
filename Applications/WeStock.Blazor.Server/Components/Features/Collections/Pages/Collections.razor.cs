using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Pages;

public partial class Collections
{
    [Inject]
    private ICollectionService CollectionService { get; set; } = null!;

    private IQueryable<CollectionViewModel>? _collections;

    protected override async Task OnInitializedAsync()
    {
        var collections = await CollectionService.RetrieveCollectionsAsync();
        _collections = collections
            .Select(collectionDto => collectionDto.MapToViewModel())
            .AsQueryable();
    }
}