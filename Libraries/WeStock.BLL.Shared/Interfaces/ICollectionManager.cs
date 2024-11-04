using WeStock.BLL.Shared.Models;

namespace WeStock.BLL.Shared.Interfaces;

public interface ICollectionManager
{
    Task<Collection> CreateCollectionAsync(Collection collection);
    Task<List<Collection>> RetrieveCollectionsAsync();
    Task<Collection?> RetrieveCollectionByIdAsync(int collectionId);
    Task<bool> UpdateCollectionAsync(Collection collection);
    Task<bool> DeleteCollectionByIdAsync(int collectionId);
    Task<int> GetItemCountInCollectionAsync(int collectionId);
}