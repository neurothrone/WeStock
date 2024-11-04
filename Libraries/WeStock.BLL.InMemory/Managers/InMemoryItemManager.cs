using WeStock.BLL.Shared.Interfaces;
using WeStock.BLL.Shared.Models;
using WeStock.BLL.Shared.Utils;
using WeStock.DAL.InMemory.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.InMemory.Managers;

public class InMemoryItemManager : IItemManager
{
    private readonly IItemRepository _repository;

    public InMemoryItemManager(IItemRepository repository)
    {
        _repository = repository;
    }

    #region IItemManager

    public async Task<Item> CreateItemAsync(Item item)
    {
        var entity = await _repository.CreateItemAsync(item.MapToCreateEntity<InMemoryItemEntity>());
        return entity.MapToModel();
    }

    public async Task<List<Item>> RetrieveItemsAsync()
    {
        var entities = await _repository.RetrieveItemsAsync();
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<List<Item>> RetrieveItemsBySectionIdAsync(int sectionId)
    {
        var entities = await _repository.RetrieveItemsBySectionIdAsync(sectionId);
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<Item?> RetrieveItemByIdAsync(int itemId)
    {
        var entity = await _repository.RetrieveItemByIdAsync(itemId);
        return entity?.MapToModel();
    }

    public Task<bool> UpdateItemAsync(Item item)
    {
        return _repository.UpdateItemAsync(item.MapToEntity<InMemoryItemEntity>());
    }

    public Task<bool> DeleteItemByIdAsync(int itemId)
    {
        return _repository.DeleteItemByIdAsync(itemId);
    }

    public Task<bool> RemoveItemFromSection(Item item)
    {
        return _repository.RemoveItemFromSection(item.MapToEntity<InMemoryItemEntity>());
    }

    public async Task<IEnumerable<Item>> SearchItemsByNameAsync(string? searchText = null)
    {
        var entities = await _repository.SearchItemsByNameAsync(searchText);
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<IEnumerable<Item>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText)
    {
        var entities = await _repository.SearchItemsByNameExcludingSectionAsync(sectionId, searchText);
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    #endregion
}