using WeStock.DAL.InMemory.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.DAL.InMemory.Data;

public class DataStore
{
    public readonly List<ICollectionEntity> Collections = [];
    public int CurrentCollectionId;

    public readonly List<ISectionEntity> Sections = [];
    public int CurrentSectionId;

    public readonly List<IItemEntity> Items = [];
    public int CurrentItemId;

    // Dictionary to map item ID:s to lists of section ID:s.
    public readonly Dictionary<int, List<int>> ItemSectionMapping = [];

    public void AddInitialData()
    {
        var collection = new InMemoryCollectionEntity { Id = ++CurrentCollectionId, Name = "Sample Collection" };
        Collections.Add(collection);

        for (int i = 1; i < 15; i++)
        {
            CurrentCollectionId += 1;
            Collections.Add(new InMemoryCollectionEntity
            {
                Id = CurrentCollectionId,
                Name = $"Sample Collection {CurrentCollectionId}"
            });
        }

        for (int i = 0; i < 2; i++)
        {
            CurrentSectionId += 1;
            var section = new InMemorySectionEntity
            {
                Id = CurrentSectionId,
                Name = $"Sample Section {CurrentSectionId}",
                CollectionId = collection.Id
            };
            Sections.Add(section);
        }

        var sectionId = Sections.First().Id;
        for (int i = 0; i < 3; i++)
        {
            CurrentItemId += 1;
            var item = new InMemoryItemEntity
            {
                Id = CurrentItemId,
                Name = $"Sample Item {CurrentItemId}",
                Quantity = CurrentItemId * 2,
                SectionId = sectionId
            };
            Items.Add(item);

            ItemSectionMapping.Add(item.Id, [sectionId]);
        }
    }
}