namespace WeStock.DAL.Shared.Interfaces;

public interface ICollectionRepository
{
    Task<ICollectionEntity> CreateCollectionAsync(ICollectionEntity collection);
    Task<List<ICollectionEntity>> RetrieveCollectionsAsync();
    Task<ICollectionEntity?> RetrieveCollectionByIdAsync(int id);
    Task<bool> UpdateCollectionAsync(ICollectionEntity collection);
    Task<bool> DeleteCollectionByIdAsync(int id);
    Task<int> GetItemCountInCollectionAsync(int collectionId);
}