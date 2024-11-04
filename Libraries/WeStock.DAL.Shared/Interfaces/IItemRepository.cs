namespace WeStock.DAL.Shared.Interfaces;

public interface IItemRepository
{
    Task<IItemEntity> CreateItemAsync(IItemEntity item);
    Task<List<IItemEntity>> RetrieveItemsAsync();
    Task<List<IItemEntity>> RetrieveItemsBySectionIdAsync(int sectionId);
    Task<IItemEntity?> RetrieveItemByIdAsync(int id);
    Task<bool> UpdateItemAsync(IItemEntity item);
    Task<bool> DeleteItemByIdAsync(int id);
    Task<bool> RemoveItemFromSection(IItemEntity item);
    Task<IEnumerable<IItemEntity>> SearchItemsByNameAsync(string? searchText = null);
    Task<IEnumerable<IItemEntity>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText);
}