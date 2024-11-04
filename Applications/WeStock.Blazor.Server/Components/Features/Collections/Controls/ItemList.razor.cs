using Microsoft.AspNetCore.Components;
using WeStock.Blazor.Server.Utils;
using WeStock.Blazor.Server.ViewModels.Items;
using WeStock.SL.Interfaces;

namespace WeStock.Blazor.Server.Components.Features.Collections.Controls;

public partial class ItemList
{
    [Parameter, EditorRequired]
    public required int SectionId { get; set; }

    [Inject]
    private IItemService ItemService { get; set; } = null!;

    private List<ItemViewModel>? _items;

    protected override async Task OnInitializedAsync()
    {
        _items = (await ItemService.RetrieveItemsBySectionIdAsync(SectionId))
            .Select(itemDto => itemDto.MapToViewModel())
            .ToList();
    }

    private async Task RemoveItemFromSection(ItemViewModel item)
    {
        var removed = await ItemService.RemoveItemFromSection(item.MapToDto(sectionId: SectionId));
        if (removed)
            _items?.Remove(item);
    }

    public async Task AddItemToSection(ItemViewModel item)
    {
        if (_items is null)
            return;

        item.SectionId = SectionId;
        if (_items.Exists(i => i.Id == item.Id))
        {
            var added = await ItemService.UpdateItemAsync(item.MapToDto());
            if (added)
                _items?.Add(item);
        }
        else
        {
            var itemDto = await ItemService.CreateItemAsync(item.MapToCreateDto());
            _items?.Add(itemDto.MapToViewModel());
        }

        StateHasChanged();
    }

    public async Task DeleteItem(ItemViewModel item)
    {
        var deleted = await ItemService.DeleteItemByIdAsync(item.Id);
        if (!deleted)
            return;

        _items?.Remove(item);
    }
}