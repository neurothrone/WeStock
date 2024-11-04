using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Items.Controls;

public partial class ItemSearchResults
{
    [Parameter]
    public string? SearchText { get; set; }

    [Inject]
    private IItemService ItemService { get; set; } = null!;

    private List<ItemViewModel>? _items;

    protected override async Task OnParametersSetAsync()
    {
        _items = (await ItemService.SearchItemsByNameAsync(SearchText))
            .Select(itemDto => itemDto.MapToViewModel())
            .ToList();
    }

    private async Task DeleteItem(ItemViewModel item)
    {
        var deleted = await ItemService.DeleteItemByIdAsync(item.Id);
        if (deleted)
            _items?.Remove(item);
    }
}