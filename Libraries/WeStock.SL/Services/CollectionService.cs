using WeStock.BLL.Shared.Interfaces;
using WeStock.DTO.Collection;
using WeStock.SL.Interfaces;
using WeStock.SL.Utils;

namespace WeStock.SL.Services;

public class CollectionService : ICollectionService, IDisposable
{
    private readonly ICollectionManager _manager;

    public CollectionService(ICollectionManager manager)
    {
        _manager = manager;
    }

    #region ICollectionService

    public Action? OnCollectionCreated { get; set; }
    public Action? OnCollectionDeleted { get; set; }

    public async Task<CollectionDto> CreateCollectionAsync(CreateCollectionDto collection)
    {
        var newCollection = await _manager.CreateCollectionAsync(collection.MapToCreateModel());
        OnCollectionCreated?.Invoke();
        return newCollection.MapToDto();
    }

    public async Task<List<CollectionDto>> RetrieveCollectionsAsync()
    {
        var collections = await _manager.RetrieveCollectionsAsync();
        return collections
            .Select(collection => collection.MapToDto())
            .ToList();
    }

    public async Task<CollectionDto?> RetrieveCollectionByIdAsync(int collectionId)
    {
        var collection = await _manager.RetrieveCollectionByIdAsync(collectionId);
        return collection?.MapToDto();
    }

    public Task<bool> UpdateCollectionAsync(CollectionDto collection)
    {
        return _manager.UpdateCollectionAsync(collection.MapToModel());
    }

    public async Task<bool> DeleteCollectionByIdAsync(int collectionId)
    {
        var deleted = await _manager.DeleteCollectionByIdAsync(collectionId);
        if (deleted)
            OnCollectionDeleted?.Invoke();
        return deleted;
    }

    public Task<int> GetItemCountInCollectionAsync(int collectionId)
    {
        return _manager.GetItemCountInCollectionAsync(collectionId);
    }

    #endregion


    #region IDisposable

    public void Dispose()
    {
        OnCollectionCreated = null;
        OnCollectionDeleted = null;
    }

    #endregion
}