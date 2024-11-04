using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Items.Pages;

public partial class Items
{
    [Inject]
    private IItemService ItemService { get; set; } = null!;

    private string _searchText = string.Empty;

    private void SearchItems(string searchText)
    {
        _searchText = searchText;
    }

    private async Task AddItem(ItemViewModel item)
    {
        _ = await ItemService.CreateItemAsync(item.MapToCreateDto(sectionId: 0));
    }
}