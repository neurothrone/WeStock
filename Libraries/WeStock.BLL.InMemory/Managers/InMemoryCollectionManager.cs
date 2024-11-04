using WeStock.BLL.Shared.Interfaces;
using WeStock.BLL.Shared.Models;
using WeStock.BLL.Shared.Utils;
using WeStock.DAL.InMemory.Entities;
using WeStock.DAL.Shared.Interfaces;

namespace WeStock.BLL.InMemory.Managers;

public class InMemoryCollectionManager : ICollectionManager
{
    private readonly ICollectionRepository _repository;

    public InMemoryCollectionManager(ICollectionRepository repository)
    {
        _repository = repository;
    }

    #region ICollectionManager

    public async Task<Collection> CreateCollectionAsync(Collection collection)
    {
        var entity = await _repository.CreateCollectionAsync(collection.MapToCreateEntity<InMemoryCollectionEntity>());
        return entity.MapToModel();
    }

    public async Task<List<Collection>> RetrieveCollectionsAsync()
    {
        var entities = await _repository.RetrieveCollectionsAsync();
        return entities
            .Select(entity => entity.MapToModel())
            .ToList();
    }

    public async Task<Collection?> RetrieveCollectionByIdAsync(int collectionId)
    {
        var entity = await _repository.RetrieveCollectionByIdAsync(collectionId);
        return entity?.MapToModel();
    }

    public Task<bool> UpdateCollectionAsync(Collection collection)
    {
        return _repository.UpdateCollectionAsync(collection.MapToEntity<InMemoryCollectionEntity>());
    }

    public Task<bool> DeleteCollectionByIdAsync(int collectionId)
    {
        return _repository.DeleteCollectionByIdAsync(collectionId);
    }

    public Task<int> GetItemCountInCollectionAsync(int collectionId)
    {
        return _repository.GetItemCountInCollectionAsync(collectionId);
    }

    #endregion
}