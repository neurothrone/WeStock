using WeStock.BLL.Shared.Interfaces;
using WeStock.DTO.Item;
using WeStock.SL.Interfaces;
using WeStock.SL.Utils;

namespace WeStock.SL.Services;

public class ItemService : IItemService, IDisposable
{
    private readonly IItemManager _manager;

    public ItemService(IItemManager manager)
    {
        _manager = manager;
    }

    #region IItemService

    public Action? OnItemCreated { get; set; }
    public Action? OnItemDeleted { get; set; }

    public async Task<ItemDto> CreateItemAsync(CreateItemDto item)
    {
        var newItem = await _manager.CreateItemAsync(item.MapToCreateModel());
        OnItemCreated?.Invoke();
        return newItem.MapToDto();
    }

    public async Task<List<ItemDto>> RetrieveItemsAsync()
    {
        var items = await _manager.RetrieveItemsAsync();
        return items
            .Select(item => item.MapToDto())
            .ToList();
    }

    public async Task<List<ItemDto>> RetrieveItemsBySectionIdAsync(int sectionId)
    {
        var items = await _manager.RetrieveItemsBySectionIdAsync(sectionId);
        return items
            .Select(item => item.MapToDto())
            .ToList();
    }

    public async Task<ItemDto?> RetrieveItemByIdAsync(int itemId)
    {
        var item = await _manager.RetrieveItemByIdAsync(itemId);
        return item?.MapToDto();
    }

    public Task<bool> UpdateItemAsync(ItemDto item)
    {
        return _manager.UpdateItemAsync(item.MapToModel());
    }

    public async Task<bool> DeleteItemByIdAsync(int itemId)
    {
        var deleted = await _manager.DeleteItemByIdAsync(itemId);
        if (deleted)
            OnItemDeleted?.Invoke();
        return deleted;
    }

    public Task<bool> RemoveItemFromSection(ItemDto item)
    {
        return _manager.RemoveItemFromSection(item.MapToModel());
    }

    public async Task<IEnumerable<ItemDto>> SearchItemsByNameAsync(
        string? searchText = null)
    {
        var items = await _manager.SearchItemsByNameAsync(searchText);
        return items
            .Select(item => item.MapToDto())
            .ToList();
    }

    public async Task<IEnumerable<ItemDto>> SearchItemsByNameExcludingSectionAsync(
        int sectionId,
        string? searchText = null)
    {
        var items = await _manager.SearchItemsByNameExcludingSectionAsync(sectionId, searchText);
        return items
            .Select(item => item.MapToDto())
            .ToList();
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        OnItemCreated = null;
        OnItemDeleted = null;
    }

    #endregion
}