using WeStock.DAL.InMemory.Data;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Repositories;

public class InMemoryCollectionRepository : ICollectionRepository
{
    private readonly DataStore _dataStore;

    public InMemoryCollectionRepository(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<ICollectionEntity> CreateCollectionAsync(ICollectionEntity inMemoryCollection)
    {
        inMemoryCollection.Id = ++_dataStore.CurrentCollectionId;
        _dataStore.Collections.Add(inMemoryCollection);

        return Task.FromResult(inMemoryCollection);
    }

    public Task<List<ICollectionEntity>> RetrieveCollectionsAsync()
    {
        return Task.FromResult(_dataStore.Collections);
    }

    public Task<ICollectionEntity?> RetrieveCollectionByIdAsync(int collectionId)
    {
        return Task.FromResult(_dataStore.Collections.FirstOrDefault(c => c.Id == collectionId));
    }

    public async Task<bool> UpdateCollectionAsync(ICollectionEntity inMemoryCollection)
    {
        var collectionToUpdate = await RetrieveCollectionByIdAsync(inMemoryCollection.Id);
        if (collectionToUpdate is null)
            return false;

        collectionToUpdate.Name = inMemoryCollection.Name;
        return true;
    }

    public async Task<bool> DeleteCollectionByIdAsync(int collectionId)
    {
        var collectionToDelete = await RetrieveCollectionByIdAsync(collectionId);
        if (collectionToDelete is null)
            return false;

        _dataStore.Collections.Remove(collectionToDelete);

        // Delete all Section:s that are related to the Collection.
        var sectionsToDelete = _dataStore.Sections
            .Where(s => s.CollectionId == collectionId)
            .ToList();
        foreach (var section in sectionsToDelete)
        {
            _dataStore.Sections.Remove(section);

            // Update all Item:s that are related to the Section.
            var itemsToUpdate = _dataStore.Items.Where(i => i.SectionId == section.Id);
            foreach (var item in itemsToUpdate)
            {
                item.SectionId = 0;
            }
        }

        return true;
    }

    public Task<int> GetItemCountInCollectionAsync(int collectionId)
    {
        int itemCount = _dataStore.Sections
            // Filter out sections that are not related to the Collection.
            .Where(section => section.CollectionId == collectionId)
            .SelectMany(section => _dataStore.ItemSectionMapping
                // Filter out items that are not related to the sections in the Collection.
                .Where(entry => entry.Value.Contains(section.Id))
                .Select(entry => entry.Key)
            )
            // Count only unique item:s since an item can be related to multiple sections.
            .Distinct()
            .Count();

        return Task.FromResult(itemCount);
    }
}