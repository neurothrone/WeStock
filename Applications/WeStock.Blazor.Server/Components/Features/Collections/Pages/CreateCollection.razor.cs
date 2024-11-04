using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Collections;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Pages;

public partial class CreateCollection
{
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ICollectionService CollectionService { get; set; } = null!;

    private readonly CollectionViewModel _collection = new();

    private async Task Create()
    {
        _ = await CollectionService.CreateCollectionAsync(_collection.MapToCreateDto());
        Navigation.NavigateTo("/collections");
    }
}