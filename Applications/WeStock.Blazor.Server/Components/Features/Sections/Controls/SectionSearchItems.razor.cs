using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Sections.Controls;

public partial class SectionSearchItems
{
    [Parameter]
    public int SectionId { get; set; }

    [Parameter]
    public EventCallback<ItemViewModel> OnItemAdded { get; set; }

    [Parameter]
    public EventCallback<ItemViewModel> OnItemDeleted { get; set; }

    [Inject]
    private IItemService ItemService { get; set; } = null!;

    private List<ItemViewModel> _items = [];

    private async Task SearchItems(string searchText)
    {
        _items = (await ItemService.SearchItemsByNameExcludingSectionAsync(SectionId, searchText))
            .Select(item => item.MapToViewModel())
            .ToList();
    }

    private async Task AddItem(ItemViewModel item)
    {
        await OnItemAdded.InvokeAsync(item);
        _items.Remove(item);
    }

    private async Task DeleteItem(ItemViewModel item)
    {
        await OnItemDeleted.InvokeAsync(item);
        _items.Remove(item);
    }
}