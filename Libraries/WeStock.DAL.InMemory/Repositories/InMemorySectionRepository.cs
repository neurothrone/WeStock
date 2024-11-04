using WeStock.DAL.InMemory.Data;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Repositories;

public class InMemorySectionRepository : ISectionRepository
{
    private readonly DataStore _dataStore;

    public InMemorySectionRepository(DataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<ISectionEntity> CreateSectionAsync(ISectionEntity inMemorySection)
    {
        inMemorySection.Id = ++_dataStore.CurrentSectionId;
        _dataStore.Sections.Add(inMemorySection);

        return Task.FromResult(inMemorySection);
    }

    public Task<List<ISectionEntity>> RetrieveSectionsAsync() => Task.FromResult(_dataStore.Sections);


    public Task<List<ISectionEntity>> RetrieveSectionsByCollectionIdAsync(int collectionId)
    {
        return Task.FromResult(_dataStore.Sections
            .Where(s => s.CollectionId == collectionId)
            .ToList());
    }

    public Task<ISectionEntity?> RetrieveSectionByIdAsync(int sectionId) => Task.FromResult(
        _dataStore.Sections.FirstOrDefault(section => section.Id == sectionId));

    public async Task<bool> UpdateSectionAsync(ISectionEntity inMemorySection)
    {
        var sectionToUpdate = await RetrieveSectionByIdAsync(inMemorySection.Id);
        if (sectionToUpdate is null)
            return false;

        sectionToUpdate.Name = inMemorySection.Name;
        return true;
    }

    public async Task<bool> DeleteSectionByIdAsync(int sectionId)
    {
        var sectionToDelete = await RetrieveSectionByIdAsync(sectionId);
        if (sectionToDelete is null)
            return false;

        // Update all Item:s that are related to the Section.
        var itemsToUpdate = _dataStore.Items.Where(i => i.SectionId == sectionId);
        foreach (var item in itemsToUpdate)
        {
            item.SectionId = 0;
        }

        return _dataStore.Sections.Remove(sectionToDelete);
    }

    public Task<int> RetrieveSectionsCountByCollectionIdAsync(int collectionId) => Task.FromResult(
        _dataStore.Sections.Count(s => s.CollectionId == collectionId));
}