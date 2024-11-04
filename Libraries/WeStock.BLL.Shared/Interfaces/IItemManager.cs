using WeStock.BLL.Shared.Models;

namespace WeStock.BLL.Shared.Interfaces;

public interface IItemManager
{
    Task<Item> CreateItemAsync(Item item);
    Task<List<Item>> RetrieveItemsAsync();
    Task<List<Item>> RetrieveItemsBySectionIdAsync(int sectionId);
    Task<Item?> RetrieveItemByIdAsync(int itemId);
    Task<bool> UpdateItemAsync(Item item);
    Task<bool> DeleteItemByIdAsync(int itemId);
    Task<bool> AddItemToSection(Item item);
    Task<bool> RemoveItemFromSection(Item item);
    Task<IEnumerable<Item>> SearchItemsByNameAsync(string? searchText = null);
    Task<IEnumerable<Item>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText);
}