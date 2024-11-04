using WeStock.DTO.Item;

namespace WeStock.SL.Interfaces;

public interface IItemService
{
    public Action? OnItemCreated { get; set; }
    public Action? OnItemDeleted { get; set; }

    Task<ItemDto> CreateItemAsync(CreateItemDto item);
    Task<List<ItemDto>> RetrieveItemsAsync();
    Task<List<ItemDto>> RetrieveItemsBySectionIdAsync(int sectionId);
    Task<ItemDto?> RetrieveItemByIdAsync(int itemId);
    Task<bool> UpdateItemAsync(ItemDto item);
    Task<bool> DeleteItemByIdAsync(int itemId);
    Task<bool> AddItemToSection(ItemDto item);
    Task<bool> RemoveItemFromSection(ItemDto item);
    Task<IEnumerable<ItemDto>> SearchItemsByNameAsync(string? searchText = null);
    Task<IEnumerable<ItemDto>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText = null);
}