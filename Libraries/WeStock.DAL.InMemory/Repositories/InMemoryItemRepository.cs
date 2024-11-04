using WeStock.DAL.InMemory.Data;
using WeStock.DAL.InMemory.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Repositories;

public class InMemoryItemRepository : IItemRepository
{
    private readonly DataStore _dataStore;

    public InMemoryItemRepository(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    #region IItemRepository

    public Task<IItemEntity> CreateItemAsync(IItemEntity inMemoryItem)
    {
        inMemoryItem.Id = ++_dataStore.CurrentItemId;
        _dataStore.Items.Add(inMemoryItem);

        if (inMemoryItem.SectionId != 0)
            AddItemToSection(inMemoryItem);

        return Task.FromResult(inMemoryItem);
    }

    public Task<List<IItemEntity>> RetrieveItemsAsync()
    {
        return Task.FromResult(_dataStore.Items);
    }

    public Task<List<IItemEntity>> RetrieveItemsBySectionIdAsync(int sectionId)
    {
        var itemIds = _dataStore.ItemSectionMapping
            .Where(entry => entry.Value.Contains(sectionId))
            .Select(entry => entry.Key)
            .ToHashSet();

        return Task.FromResult(_dataStore.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToList());
    }

    public Task<IItemEntity?> RetrieveItemByIdAsync(int itemId)
    {
        return Task.FromResult(_dataStore.Items
            .FirstOrDefault(item => item.Id == itemId));
    }

    public Task<bool> UpdateItemAsync(IItemEntity inMemoryItem)
    {
        var itemToUpdate = _dataStore.Items.FirstOrDefault(i => i.Id == inMemoryItem.Id);
        if (itemToUpdate is null)
            return Task.FromResult(false);

        itemToUpdate.Name = inMemoryItem.Name;
        itemToUpdate.Quantity = inMemoryItem.Quantity;

        return Task.FromResult(true);
    }

    public async Task<bool> DeleteItemByIdAsync(int itemId)
    {
        var itemToDelete = await RetrieveItemByIdAsync(itemId);
        if (itemToDelete is null)
            return false;

        _dataStore.ItemSectionMapping.Remove(itemToDelete.Id);
        return _dataStore.Items.Remove(itemToDelete);
    }

    public Task<bool> AddItemToSection(IItemEntity item)
    {
        if (!_dataStore.ItemSectionMapping.ContainsKey(item.Id))
            _dataStore.ItemSectionMapping[item.Id] = [];

        var storedItem = _dataStore.Items.FirstOrDefault(i => i.Id == item.Id);
        if (storedItem is null)
            return Task.FromResult(false);

        storedItem.SectionId = item.SectionId;
        _dataStore.ItemSectionMapping[item.Id].Add(item.SectionId);

        return Task.FromResult(true);
    }

    public Task<bool> RemoveItemFromSection(IItemEntity item)
    {
        if (!_dataStore.ItemSectionMapping.ContainsKey(item.Id))
            return Task.FromResult(false);

        var removed = _dataStore.ItemSectionMapping[item.Id].Remove(item.SectionId);
        if (removed)
        {
            var storedItem = _dataStore.Items.FirstOrDefault(i => i.Id == item.Id);
            if (storedItem is not null)
                storedItem.SectionId = 0;
        }

        // Remove dictionary entry if there are no items in the list.
        if (_dataStore.ItemSectionMapping[item.Id].Count == 0)
            _dataStore.ItemSectionMapping.Remove(item.Id);

        return Task.FromResult(removed);
    }

    public Task<IEnumerable<IItemEntity>> SearchItemsByNameAsync(string? searchText = null)
    {
        return Task.FromResult(string.IsNullOrWhiteSpace(searchText)
            ? []
            : _dataStore.Items.Where(item => item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
    }

    // public Task<IEnumerable<IItemEntity>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText)
    // {
    //     if (string.IsNullOrWhiteSpace(searchText))
    //         searchText = string.Empty;
    //
    //     var excludedItemIds = _dataStore.ItemSectionMapping
    //         .Where(entry => entry.Value.Contains(sectionId))
    //         .Select(entry => entry.Key)
    //         .ToHashSet();
    //
    //     return Task.FromResult(
    //         _dataStore.Items
    //             // Filter out all items that are related to the section to exclude.
    //             .Where(item => !excludedItemIds.Contains(item.Id))
    //             .Where(item => item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
    // }

    public Task<IEnumerable<IItemEntity>> SearchItemsByNameExcludingSectionAsync(int sectionId, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            searchText = string.Empty;

        var excludedItemIds = _dataStore.ItemSectionMapping
            .Where(entry => entry.Value.Contains(sectionId))
            .Select(entry => entry.Key)
            .ToHashSet();

        var itemEntities = _dataStore.Items
            // Filter out all items that are related to the section to exclude.
            .Where(item => !excludedItemIds.Contains(item.Id))
            .Where(item => item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            // Projecting to an anonymous type and excluding the SectionId property.
            .Select(item => new
            {
                item.Id,
                item.Name,
                item.Quantity
            })
            .ToList();

        return Task.FromResult<IEnumerable<IItemEntity>>(
            itemEntities.Select(item =>
                new InMemoryItemEntity
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity
                })
        );
    }

    #endregion
}